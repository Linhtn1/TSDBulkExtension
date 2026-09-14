using System.Text;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Streaming;

namespace TSD.BulkExtensions.SqlServer;

/// <summary>
/// T-SQL text generation for the SQL Server adapter. Pure functions over <see cref="EntityTableMap"/>; no I/O.
/// </summary>
internal static class SqlServerSqlBuilder
{
    /// <summary>Name of the MERGE action column in the output table.</summary>
    public const string ActionColumnName = "__Action";

    /// <summary>Quotes an identifier as <c>[name]</c>, escaping embedded closing brackets.</summary>
    public static string Quote(string identifier)
        => "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";

    /// <summary>
    /// Quoted name of the destination table. Without a model schema the name stays unqualified, exactly as EF's own SQL,
    /// so the login's default schema applies.
    /// </summary>
    public static string TableName(EntityTableMap map)
        => map.Schema is null ? Quote(map.TableName) : Quote(map.Schema) + "." + Quote(map.TableName);

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

    /// <summary>Session-local temp table name for staging, unique per operation.</summary>
    public static string StagingTableName(EntityTableMap map, string suffix) => Quote($"#TSD_{map.TableName}_{suffix}");

    /// <summary>Session-local temp table name for MERGE output, unique per operation.</summary>
    public static string OutputTableName(EntityTableMap map, string suffix) => Quote($"#TSD_{map.TableName}_{suffix}_Out");

    /// <summary>
    /// Staging table with the given columns (all nullable, store types copied from the model) plus the row index as
    /// clustered primary key, so the ordered MERGE source needs no sort. An explicit CREATE TABLE avoids inheriting
    /// IDENTITY or computed definitions from the target.
    /// </summary>
    public static string CreateStagingTable(string name, IReadOnlyList<ColumnMap> columns)
    {
        var sb = new StringBuilder("CREATE TABLE ").Append(name).Append(" (");
        foreach (var column in columns)
        {
            sb.Append(Quote(column.ColumnName)).Append(' ').Append(TempStoreType(column)).Append(" NULL, ");
        }

        sb.Append(Quote(EntityDataReader<object>.IndexColumnName)).Append(" int NOT NULL PRIMARY KEY CLUSTERED);");
        return sb.ToString();
    }

    /// <summary>Output table: source index, MERGE action, then the server-generated columns to read back.</summary>
    public static string CreateOutputTable(string name, IReadOnlyList<ColumnMap> columns)
    {
        var sb = new StringBuilder("CREATE TABLE ").Append(name).Append(" (")
            .Append(Quote(EntityDataReader<object>.IndexColumnName)).Append(" int NULL, ")
            .Append(Quote(ActionColumnName)).Append(" char(1) NOT NULL");
        foreach (var column in columns)
        {
            sb.Append(", ").Append(Quote(column.ColumnName)).Append(' ').Append(TempStoreType(column)).Append(" NULL");
        }

        sb.Append(");");
        return sb.ToString();
    }

    /// <summary>rowversion cannot be declared on a temp table, so it travels as varbinary(8).</summary>
    private static string TempStoreType(ColumnMap column)
        => column.IsRowVersion || column.StoreType.Equals("rowversion", StringComparison.OrdinalIgnoreCase) || column.StoreType.Equals("timestamp", StringComparison.OrdinalIgnoreCase)
            ? "varbinary(8)"
            : column.StoreType;

    /// <summary>One batch dropping every listed temp table that still exists.</summary>
    public static string DropTempTables(IEnumerable<string> names)
    {
        var sb = new StringBuilder();
        foreach (var name in names)
        {
            sb.Append("IF OBJECT_ID('tempdb..").Append(name.Trim('[', ']')).Append("') IS NOT NULL DROP TABLE ").Append(name).Append("; ");
        }

        return sb.ToString().TrimEnd();
    }

    public static string Truncate(EntityTableMap map) => $"TRUNCATE TABLE {TableName(map)};";

    public static string IdentityInsert(EntityTableMap map, bool on) => $"SET IDENTITY_INSERT {TableName(map)} {(on ? "ON" : "OFF")};";

    /// <summary>Join predicate on the match columns, null-safe when any of them is nullable.</summary>
    public static string MatchPredicate(EntityTableMap map, string target, string source)
    {
        var sb = new StringBuilder();
        foreach (var column in map.MatchColumns)
        {
            if (sb.Length > 0)
            {
                sb.Append(" AND ");
            }

            var t = target + "." + Quote(column.ColumnName);
            var s = source + "." + Quote(column.ColumnName);
            sb.Append(column.IsNullable
                ? $"({t} = {s} OR ({t} IS NULL AND {s} IS NULL))"
                : $"{t} = {s}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// The MERGE statement for every staged operation. <paramref name="insertColumns"/> is the INSERT column list
    /// (identity and server-defaulted columns already removed); <paramref name="outputColumns"/> are read back when
    /// <paramref name="outputTable"/> is given.
    /// </summary>
    public static string Merge(
        EntityTableMap map,
        OperationType operationType,
        BulkConfig config,
        string stagingTable,
        int rowCount,
        IReadOnlyList<ColumnMap> insertColumns,
        string? outputTable,
        IReadOnlyList<ColumnMap> outputColumns)
    {
        var index = Quote(EntityDataReader<object>.IndexColumnName);
        var sb = new StringBuilder();

        var inserts = operationType is OperationType.Insert or OperationType.InsertOrUpdate or OperationType.InsertOrUpdateOrDelete;
        var updates = operationType is OperationType.Update or OperationType.InsertOrUpdate or OperationType.InsertOrUpdateOrDelete;
        var needsNoOpUpdate = updates && map.UpdateColumns.Count == 0 && outputTable is not null && outputColumns.Count > 0;

        if (needsNoOpUpdate)
        {
            // Assigning a variable is a legal UPDATE SET target that touches no column, so matched rows still reach OUTPUT.
            sb.Append("DECLARE @tsd_noop int; ");
        }

        sb.Append("MERGE ").Append(TableName(map));
        if (config.WithHoldlock)
        {
            sb.Append(" WITH (HOLDLOCK)");
        }

        sb.Append(" AS T USING ");
        if (config.PreserveInsertOrder)
        {
            // TOP + ORDER BY makes SQL Server walk the source in list order, so identities come out ascending.
            sb.Append("(SELECT TOP (").Append(rowCount).Append(") * FROM ").Append(stagingTable).Append(" ORDER BY ").Append(index).Append(')');
        }
        else
        {
            sb.Append(stagingTable);
        }

        sb.Append(" AS S ON ");
        sb.Append(operationType == OperationType.Insert ? "1 = 0" : MatchPredicate(map, "T", "S"));

        if (inserts)
        {
            sb.Append(" WHEN NOT MATCHED BY TARGET THEN ");
            sb.Append(insertColumns.Count == 0
                ? "INSERT DEFAULT VALUES"
                : $"INSERT ({ColumnList(insertColumns)}) VALUES ({ColumnList(insertColumns, "S")})");
        }

        if (updates)
        {
            if (map.UpdateColumns.Count > 0)
            {
                sb.Append(" WHEN MATCHED");
                if (map.CompareColumns.Count > 0)
                {
                    sb.Append(" AND EXISTS (SELECT ").Append(ColumnList(map.CompareColumns, "S"))
                      .Append(" EXCEPT SELECT ").Append(ColumnList(map.CompareColumns, "T")).Append(')');
                }

                if (config.OnConflictUpdateWhereSql is not null)
                {
                    sb.Append(" AND (").Append(config.OnConflictUpdateWhereSql("T", "S")).Append(')');
                }

                sb.Append(" THEN UPDATE SET ");
                var first = true;
                foreach (var column in map.UpdateColumns)
                {
                    if (!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("T.").Append(Quote(column.ColumnName)).Append(" = S.").Append(Quote(column.ColumnName));
                }
            }
            else if (needsNoOpUpdate)
            {
                sb.Append(" WHEN MATCHED THEN UPDATE SET @tsd_noop = 1");
            }
        }

        if (operationType == OperationType.Delete)
        {
            sb.Append(" WHEN MATCHED THEN DELETE");
        }

        if (operationType == OperationType.InsertOrUpdateOrDelete)
        {
            sb.Append(" WHEN NOT MATCHED BY SOURCE THEN DELETE");
        }

        if (outputTable is not null)
        {
            sb.Append(" OUTPUT S.").Append(index).Append(", SUBSTRING($action, 1, 1)");
            foreach (var column in outputColumns)
            {
                sb.Append(", INSERTED.").Append(Quote(column.ColumnName));
            }

            sb.Append(" INTO ").Append(outputTable).Append(" (").Append(index).Append(", ").Append(Quote(ActionColumnName));
            foreach (var column in outputColumns)
            {
                sb.Append(", ").Append(Quote(column.ColumnName));
            }

            sb.Append(')');
        }

        sb.Append(';');
        return sb.ToString();
    }

    /// <summary>Reads the output table back: index, action, then the generated columns.</summary>
    public static string SelectOutput(string outputTable, IReadOnlyList<ColumnMap> outputColumns)
    {
        var sb = new StringBuilder("SELECT ")
            .Append(Quote(EntityDataReader<object>.IndexColumnName)).Append(", ")
            .Append(Quote(ActionColumnName));
        foreach (var column in outputColumns)
        {
            sb.Append(", ").Append(Quote(column.ColumnName));
        }

        return sb.Append(" FROM ").Append(outputTable).Append(';').ToString();
    }

    /// <summary>
    /// BulkRead: current table values for every staged key, tagged with the source index and a constant 'R' action so the
    /// result has the same shape as <see cref="SelectOutput"/>.
    /// </summary>
    public static string SelectRead(EntityTableMap map, string stagingTable, IReadOnlyList<ColumnMap> columns)
    {
        var index = Quote(EntityDataReader<object>.IndexColumnName);
        var columnList = columns.Count == 0 ? string.Empty : ", " + ColumnList(columns, "T");
        return $"SELECT S.{index}, 'R' AS {Quote(ActionColumnName)}{columnList} FROM {TableName(map)} AS T INNER JOIN {stagingTable} AS S ON {MatchPredicate(map, "T", "S")};";
    }
}
