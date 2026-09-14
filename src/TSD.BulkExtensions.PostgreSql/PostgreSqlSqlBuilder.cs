using System.Text;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Streaming;

namespace TSD.BulkExtensions.PostgreSql;

/// <summary>
/// SQL text generation for the PostgreSQL adapter. Pure functions over <see cref="EntityTableMap"/>; no I/O.
/// Set operations are split into UPDATE … FROM / INSERT … SELECT … WHERE NOT EXISTS / DELETE … USING so that
/// generated keys can be assigned in the staging table up front and mapped back by row index without relying on
/// RETURNING order (PostgreSQL gives no ordering guarantee for it).
/// </summary>
/// <remarks>
/// Match predicates use <c>IS NOT DISTINCT FROM</c> only for the columns named in <c>nullSafeKeys</c>: PostgreSQL cannot
/// hash-join or use an index on that operator, so callers pass the nullable match columns that actually carry a NULL in
/// the staged rows. When the set is <c>null</c> every nullable match column is treated null-safe.
/// </remarks>
internal static class PostgreSqlSqlBuilder
{
    /// <summary>Quoted name of the row index column.</summary>
    public static readonly string Index = Quote(EntityDataReader<object>.IndexColumnName);

    /// <summary>Quotes an identifier as <c>"name"</c>, escaping embedded double quotes.</summary>
    public static string Quote(string identifier)
        => "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";

    /// <summary>Quoted name of the destination table; without a model schema the name stays unqualified (search_path applies).</summary>
    public static string TableName(EntityTableMap map)
        => map.Schema is null ? Quote(map.TableName) : Quote(map.Schema) + "." + Quote(map.TableName);

    /// <summary>The table name as a string literal for <c>pg_get_serial_sequence</c>.</summary>
    public static string TableNameLiteral(EntityTableMap map)
        => "'" + TableName(map).Replace("'", "''", StringComparison.Ordinal) + "'";

    /// <summary>Comma-separated quoted column list, optionally prefixed with an alias.</summary>
    public static string ColumnList(IEnumerable<ColumnMap> columns, string? alias = null)
    {
        var sb = new StringBuilder();
        foreach (var column in columns)
        {
            if (sb.Length > 0)
            {
                sb.Append(", ");
            }

            if (alias is not null)
            {
                sb.Append(alias).Append('.');
            }

            sb.Append(Quote(column.ColumnName));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Session-local temp table name for staging, unique per operation. PostgreSQL truncates identifiers to 63 bytes, so
    /// the table part is shortened first to keep the unique suffix intact.
    /// </summary>
    public static string StagingTableName(EntityTableMap map, string suffix)
    {
        var table = map.TableName.Length > 40 ? map.TableName[..40] : map.TableName;
        return Quote($"tsd_stg_{table}_{suffix}");
    }

    /// <summary>
    /// Staging table with the given columns (all nullable, store types copied from the model) plus the row index as primary
    /// key. With <paramref name="onCommitDrop"/> the table disappears with the transaction, saving the explicit DROP.
    /// </summary>
    public static string CreateStagingTable(string name, IReadOnlyList<ColumnMap> columns, bool onCommitDrop)
    {
        var sb = new StringBuilder("CREATE TEMPORARY TABLE ").Append(name).Append(" (");
        foreach (var column in columns)
        {
            sb.Append(Quote(column.ColumnName)).Append(' ').Append(column.StoreType).Append(" NULL, ");
        }

        sb.Append(Index).Append(" integer NOT NULL PRIMARY KEY)");
        if (onCommitDrop)
        {
            sb.Append(" ON COMMIT DROP");
        }

        return sb.Append(';').ToString();
    }

    public static string DropStagingTable(string name) => $"DROP TABLE IF EXISTS {name};";

    /// <summary>COPY command for the binary importer: the staging (or destination) columns, plus the index for staging.</summary>
    public static string Copy(string table, IReadOnlyList<ColumnMap> columns, bool includeIndex)
    {
        var list = ColumnList(columns);
        if (includeIndex)
        {
            list = list.Length == 0 ? Index : list + ", " + Index;
        }

        return $"COPY {table} ({list}) FROM STDIN (FORMAT BINARY)";
    }

    /// <summary>The sequence behind an identity/serial column, as a regclass expression (NULL when the table owns none).</summary>
    public static string Sequence(EntityTableMap map, ColumnMap identity)
        => $"pg_get_serial_sequence({TableNameLiteral(map)}, '{identity.ColumnName.Replace("'", "''", StringComparison.Ordinal)}')::regclass";

    /// <summary>
    /// Assigns fresh sequence values to the identity column of the staged rows that will be inserted, in row-index order,
    /// so every new row carries its key before the INSERT. With <paramref name="onlyUnmatched"/> only rows without a match
    /// in the table get a value (the upsert insert branch); otherwise every row does, so ids the caller left in the
    /// entities are ignored exactly as they are without KeepIdentity on SQL Server.
    /// </summary>
    public static string AssignIdentity(EntityTableMap map, string staging, ColumnMap identity, bool onlyUnmatched, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
    {
        var column = Quote(identity.ColumnName);
        var filter = onlyUnmatched
            ? $" WHERE NOT EXISTS (SELECT 1 FROM {TableName(map)} AS t WHERE {MatchPredicate(map.MatchColumns, "t", "o", nullSafeKeys)})"
            : string.Empty;

        // nextval() runs on the rows as they leave the ordered subquery, which ties the sequence values to the index order.
        return $"UPDATE {staging} AS s SET {column} = n.{column} FROM (SELECT o.{Index}, nextval({Sequence(map, identity)}) AS {column} FROM (SELECT {Index} FROM {staging} AS o{filter} ORDER BY {Index}) AS o) AS n WHERE n.{Index} = s.{Index};";
    }

    /// <summary>
    /// Fills a server-defaulted column of every staged row with the column's default expression (for example
    /// gen_random_uuid()). Only used when no entity supplied a value, so the whole table is rewritten.
    /// </summary>
    public static string AssignDefault(string staging, ColumnMap column, string defaultSql)
        => $"UPDATE {staging} SET {Quote(column.ColumnName)} = {defaultSql};";

    /// <summary>
    /// Moves the identity sequence past the largest key in the table after explicit keys were inserted (KeepIdentity), so
    /// later inserts do not collide. SQL Server reseeds automatically; PostgreSQL does not.
    /// </summary>
    public static string SyncSequence(EntityTableMap map, ColumnMap identity)
    {
        var sequence = Sequence(map, identity);
        return $"SELECT setval({sequence}, m.v) FROM (SELECT MAX({Quote(identity.ColumnName)}) AS v FROM {TableName(map)}) AS m WHERE m.v > COALESCE(pg_sequence_last_value({sequence}), 0);";
    }

    /// <summary>Join predicate on the match columns; see the class remarks for <paramref name="nullSafeKeys"/>.</summary>
    public static string MatchPredicate(IReadOnlyList<ColumnMap> keys, string target, string source, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
    {
        var sb = new StringBuilder();
        foreach (var column in keys)
        {
            if (sb.Length > 0)
            {
                sb.Append(" AND ");
            }

            var t = target + "." + Quote(column.ColumnName);
            var s = source + "." + Quote(column.ColumnName);
            var nullSafe = nullSafeKeys is null ? column.IsNullable : nullSafeKeys.Contains(column);
            sb.Append(nullSafe ? $"{t} IS NOT DISTINCT FROM {s}" : $"{t} = {s}");
        }

        return sb.ToString();
    }

    /// <summary>UPDATE of matched rows from the staging table, only where a compared column differs.</summary>
    public static string Update(EntityTableMap map, BulkConfig config, string staging, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
    {
        var sb = new StringBuilder("UPDATE ").Append(TableName(map)).Append(" AS t SET ");
        var first = true;
        foreach (var column in map.UpdateColumns)
        {
            if (!first)
            {
                sb.Append(", ");
            }

            first = false;
            sb.Append(Quote(column.ColumnName)).Append(" = s.").Append(Quote(column.ColumnName));
        }

        sb.Append(" FROM ").Append(staging).Append(" AS s WHERE ").Append(MatchPredicate(map.MatchColumns, "t", "s", nullSafeKeys));

        if (map.CompareColumns.Count > 0)
        {
            sb.Append(" AND (").Append(ColumnList(map.CompareColumns, "s")).Append(") IS DISTINCT FROM (").Append(ColumnList(map.CompareColumns, "t")).Append(')');
        }

        if (config.OnConflictUpdateWhereSql is not null)
        {
            sb.Append(" AND (").Append(config.OnConflictUpdateWhereSql("t", "s")).Append(')');
        }

        return sb.Append(';').ToString();
    }

    /// <summary>
    /// INSERT of staged rows. With <paramref name="onlyUnmatched"/> only rows without a match in the table are inserted
    /// (the upsert insert branch). <paramref name="overrideSystemValue"/> is needed when an identity column declared
    /// GENERATED ALWAYS receives explicit values. <paramref name="orderByIndex"/> feeds the rows in list order, which only
    /// matters when the server assigns the identity during this INSERT (PreserveInsertOrder).
    /// </summary>
    public static string Insert(EntityTableMap map, string staging, IReadOnlyList<ColumnMap> insertColumns, bool onlyUnmatched, bool overrideSystemValue, bool orderByIndex, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
    {
        var sb = new StringBuilder("INSERT INTO ").Append(TableName(map));
        if (insertColumns.Count > 0)
        {
            sb.Append(" (").Append(ColumnList(insertColumns)).Append(')');
        }

        if (overrideSystemValue)
        {
            sb.Append(" OVERRIDING SYSTEM VALUE");
        }

        sb.Append(insertColumns.Count == 0 ? " SELECT" : " SELECT " + ColumnList(insertColumns, "s"))
          .Append(" FROM ").Append(staging).Append(" AS s");

        if (onlyUnmatched)
        {
            sb.Append(" WHERE NOT EXISTS (SELECT 1 FROM ").Append(TableName(map)).Append(" AS t WHERE ").Append(MatchPredicate(map.MatchColumns, "t", "s", nullSafeKeys)).Append(')');
        }

        if (orderByIndex)
        {
            sb.Append(" ORDER BY s.").Append(Index);
        }

        return sb.Append(';').ToString();
    }

    /// <summary>DELETE of the rows the staging table names.</summary>
    public static string Delete(EntityTableMap map, string staging, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
        => $"DELETE FROM {TableName(map)} AS t USING {staging} AS s WHERE {MatchPredicate(map.MatchColumns, "t", "s", nullSafeKeys)};";

    /// <summary>DELETE of every table row the staging table does not name (the sync delete branch).</summary>
    public static string DeleteUnmatched(EntityTableMap map, string staging, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
        => $"DELETE FROM {TableName(map)} AS t WHERE NOT EXISTS (SELECT 1 FROM {staging} AS s WHERE {MatchPredicate(map.MatchColumns, "t", "s", nullSafeKeys)});";

    /// <summary>
    /// Reads generated (or, for BulkRead, requested) columns for every staged row, tagged with its index and a constant
    /// 'R' action so the shape matches the SQL Server output reader.
    /// </summary>
    public static string SelectByKey(EntityTableMap map, string staging, IReadOnlyList<ColumnMap> keys, IReadOnlyList<ColumnMap> columns, IReadOnlySet<ColumnMap>? nullSafeKeys = null)
    {
        var columnList = columns.Count == 0 ? string.Empty : ", " + ColumnList(columns, "t");
        return $"SELECT s.{Index}, 'R'{columnList} FROM {staging} AS s INNER JOIN {TableName(map)} AS t ON {MatchPredicate(keys, "t", "s", nullSafeKeys)};";
    }

    /// <summary>Reads columns straight from the staging table when every generated value was assigned there (no join needed).</summary>
    public static string SelectStaged(string staging, IReadOnlyList<ColumnMap> columns)
        => $"SELECT s.{Index}, 'R', {ColumnList(columns, "s")} FROM {staging} AS s;";

    public static string Truncate(EntityTableMap map) => $"TRUNCATE TABLE {TableName(map)};";

    /// <summary>PostgreSQL type name for the binary importer: the store type without its length/precision arguments.</summary>
    public static string CopyTypeName(ColumnMap column)
    {
        var storeType = column.StoreType;
        var open = storeType.IndexOf('(', StringComparison.Ordinal);
        if (open < 0)
        {
            return storeType;
        }

        var close = storeType.IndexOf(')', open);
        return (storeType[..open] + storeType[(close + 1)..]).Trim();
    }
}
