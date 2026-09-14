using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.PostgreSql;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.PostgreSql;

/// <summary>Shape of the generated PostgreSQL statements, without executing them.</summary>
[Collection(PostgreSqlCollection.Name)]
public sealed class SqlBuilderTests : ProviderTestBase
{
    private readonly PostgreSqlFixture _fixture;

    public SqlBuilderTests(PostgreSqlFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();

    private static (EntityTableMap Map, BulkConfig Config) Build(TestDbContext context, OperationType op, Action<BulkConfig>? configure = null)
        => BuildMap(context, typeof(Item), op, configure);

    [Fact]
    public void Quote_escapes_double_quotes_and_schema_less_tables_stay_unqualified()
    {
        using var context = CreateContext();
        var (map, _) = Build(context, OperationType.Insert);

        Assert.Equal("\"we\"\"ird\"", PostgreSqlSqlBuilder.Quote("we\"ird"));
        Assert.Equal("\"Item\"", PostgreSqlSqlBuilder.TableName(map));
        Assert.Equal("'\"Item\"'", PostgreSqlSqlBuilder.TableNameLiteral(map));
    }

    [Fact]
    public void Staging_copy_and_key_assignment_statements()
    {
        using var context = CreateContext();
        var (map, _) = Build(context, OperationType.InsertOrUpdate, o => o.SetOutputIdentity = true);

        var create = PostgreSqlSqlBuilder.CreateStagingTable("\"stg\"", map.WriteColumns, onCommitDrop: true);
        Assert.StartsWith("CREATE TEMPORARY TABLE \"stg\" (\"Id\" bigint NULL, ", create, StringComparison.Ordinal);
        Assert.EndsWith("\"__Index\" integer NOT NULL PRIMARY KEY) ON COMMIT DROP;", create, StringComparison.Ordinal);
        Assert.EndsWith("PRIMARY KEY);", PostgreSqlSqlBuilder.CreateStagingTable("\"stg\"", map.WriteColumns, onCommitDrop: false), StringComparison.Ordinal);

        var copy = PostgreSqlSqlBuilder.Copy("\"stg\"", map.WriteColumns, includeIndex: true);
        Assert.StartsWith("COPY \"stg\" (\"Id\", ", copy, StringComparison.Ordinal);
        Assert.EndsWith(", \"__Index\") FROM STDIN (FORMAT BINARY)", copy, StringComparison.Ordinal);

        Assert.Equal(
            "UPDATE \"stg\" AS s SET \"Id\" = n.\"Id\" FROM (SELECT o.\"__Index\", nextval(pg_get_serial_sequence('\"Item\"', 'Id')::regclass) AS \"Id\" "
            + "FROM (SELECT \"__Index\" FROM \"stg\" AS o ORDER BY \"__Index\") AS o) AS n WHERE n.\"__Index\" = s.\"__Index\";",
            PostgreSqlSqlBuilder.AssignIdentity(map, "\"stg\"", map.IdentityColumn!, onlyUnmatched: false));
        Assert.Contains(
            "FROM (SELECT \"__Index\" FROM \"stg\" AS o WHERE NOT EXISTS (SELECT 1 FROM \"Item\" AS t WHERE t.\"Id\" = o.\"Id\") ORDER BY \"__Index\") AS o)",
            PostgreSqlSqlBuilder.AssignIdentity(map, "\"stg\"", map.IdentityColumn!, onlyUnmatched: true),
            StringComparison.Ordinal);
        Assert.Equal(
            "UPDATE \"stg\" SET \"Id\" = gen_random_uuid();",
            PostgreSqlSqlBuilder.AssignDefault("\"stg\"", map.IdentityColumn!, "gen_random_uuid()"));
        Assert.Equal(
            "SELECT setval(pg_get_serial_sequence('\"Item\"', 'Id')::regclass, m.v) FROM (SELECT MAX(\"Id\") AS v FROM \"Item\") AS m "
            + "WHERE m.v > COALESCE(pg_sequence_last_value(pg_get_serial_sequence('\"Item\"', 'Id')::regclass), 0);",
            PostgreSqlSqlBuilder.SyncSequence(map, map.IdentityColumn!));
        Assert.Equal("character varying", PostgreSqlSqlBuilder.CopyTypeName(map.FindColumn(nameof(Item.Name))!));
        Assert.Equal("numeric", PostgreSqlSqlBuilder.CopyTypeName(map.FindColumn(nameof(Item.Price))!));
    }

    [Fact]
    public void Update_only_touches_rows_whose_compared_columns_differ()
    {
        using var context = CreateContext();
        var (map, config) = Build(context, OperationType.Update, o => o.OnConflictUpdateWhereSql = (t, s) => $"{s}.\"TimeUpdated\" > {t}.\"TimeUpdated\"");

        var sql = PostgreSqlSqlBuilder.Update(map, config, "\"stg\"");

        Assert.StartsWith("UPDATE \"Item\" AS t SET \"Description\" = s.\"Description\", ", sql, StringComparison.Ordinal);
        Assert.Contains(" FROM \"stg\" AS s WHERE t.\"Id\" = s.\"Id\" AND (s.\"Description\", ", sql, StringComparison.Ordinal);
        Assert.Contains(") IS DISTINCT FROM (t.\"Description\", ", sql, StringComparison.Ordinal);
        Assert.EndsWith(" AND (s.\"TimeUpdated\" > t.\"TimeUpdated\");", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("\"Id\" = s.\"Id\",", sql, StringComparison.Ordinal);
    }

    [Fact]
    public void Insert_skips_matches_for_upserts_and_orders_only_when_the_server_assigns_keys()
    {
        using var context = CreateContext();
        var (map, _) = Build(context, OperationType.InsertOrUpdate);
        var (insert, _) = map.GetInsertColumns(new List<Item> { new() }, new BulkValueContext(context));

        var upsert = PostgreSqlSqlBuilder.Insert(map, "\"stg\"", insert, onlyUnmatched: true, overrideSystemValue: false, orderByIndex: true);
        Assert.StartsWith("INSERT INTO \"Item\" (\"Description\", ", upsert, StringComparison.Ordinal);
        Assert.DoesNotContain("(\"Id\"", upsert, StringComparison.Ordinal);
        Assert.Contains(" SELECT s.\"Description\", ", upsert, StringComparison.Ordinal);
        Assert.Contains(" FROM \"stg\" AS s WHERE NOT EXISTS (SELECT 1 FROM \"Item\" AS t WHERE t.\"Id\" = s.\"Id\") ORDER BY s.\"__Index\";", upsert, StringComparison.Ordinal);

        var plain = PostgreSqlSqlBuilder.Insert(map, "\"stg\"", insert, onlyUnmatched: false, overrideSystemValue: true, orderByIndex: false);
        Assert.Contains(") OVERRIDING SYSTEM VALUE SELECT ", plain, StringComparison.Ordinal);
        Assert.DoesNotContain("NOT EXISTS", plain, StringComparison.Ordinal);
        Assert.DoesNotContain("ORDER BY", plain, StringComparison.Ordinal);
    }

    [Fact]
    public void Delete_sync_delete_and_read_back_statements()
    {
        using var context = CreateContext();
        var (map, _) = Build(context, OperationType.InsertOrUpdate, o => o.SetOutputIdentity = true);

        Assert.Equal("DELETE FROM \"Item\" AS t USING \"stg\" AS s WHERE t.\"Id\" = s.\"Id\";", PostgreSqlSqlBuilder.Delete(map, "\"stg\""));
        Assert.Equal("DELETE FROM \"Item\" AS t WHERE NOT EXISTS (SELECT 1 FROM \"stg\" AS s WHERE t.\"Id\" = s.\"Id\");", PostgreSqlSqlBuilder.DeleteUnmatched(map, "\"stg\""));
        Assert.Equal(
            "SELECT s.\"__Index\", 'R', t.\"Id\" FROM \"stg\" AS s INNER JOIN \"Item\" AS t ON t.\"Id\" = s.\"Id\";",
            PostgreSqlSqlBuilder.SelectByKey(map, "\"stg\"", map.PrimaryKeyColumns, map.ServerGeneratedColumns));
        Assert.Equal(
            "SELECT s.\"__Index\", 'R', s.\"Id\" FROM \"stg\" AS s;",
            PostgreSqlSqlBuilder.SelectStaged("\"stg\"", map.ServerGeneratedColumns));
    }

    [Fact]
    public void Staging_table_name_keeps_its_unique_suffix_under_the_identifier_limit()
    {
        using var context = CreateContext();
        var (map, _) = Build(context, OperationType.Insert, o => o.CustomDestinationTableName = new string('x', 80));

        var name = PostgreSqlSqlBuilder.StagingTableName(map, "abcd1234");

        Assert.EndsWith("_abcd1234\"", name, StringComparison.Ordinal);
        Assert.True(name.Length - 2 <= 63, name);
    }

    [Fact]
    public void Nullable_match_columns_are_null_safe_only_when_asked()
    {
        using var context = CreateContext();
        var (map, _) = Build(context, OperationType.Update, o => o.UpdateByProperties = new List<string> { nameof(Item.Description) });

        Assert.Equal("t.\"Description\" IS NOT DISTINCT FROM s.\"Description\"", PostgreSqlSqlBuilder.MatchPredicate(map.MatchColumns, "t", "s"));
        Assert.Equal("t.\"Description\" = s.\"Description\"", PostgreSqlSqlBuilder.MatchPredicate(map.MatchColumns, "t", "s", new HashSet<ColumnMap>()));
        Assert.Equal("t.\"Description\" IS NOT DISTINCT FROM s.\"Description\"", PostgreSqlSqlBuilder.MatchPredicate(map.MatchColumns, "t", "s", new HashSet<ColumnMap> { map.MatchColumns[0] }));
    }
}
