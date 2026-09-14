using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.SqlServer;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

/// <summary>Shape of the generated T-SQL, without executing it.</summary>
public sealed class SqlBuilderTests : SqlServerTestBase
{
    public SqlBuilderTests(SqlServerFixture fixture) : base(fixture)
    {
    }

    private (EntityTableMap Map, BulkConfig Config, BulkValueContext Values) Build(TestDbContext context, OperationType op, Action<BulkConfig>? configure = null)
    {
        var config = new BulkConfig();
        configure?.Invoke(config);
        config.Prepare(op);
        var adapter = BulkAdapterRegistry.Resolve(context);
        return (EntityTableMap.Create(context, typeof(Item), config, op, adapter.IsIdentityColumn), config, new BulkValueContext(context));
    }

    [Fact]
    public void Quote_escapes_closing_brackets_and_schema_less_tables_stay_unqualified()
    {
        using var context = CreateContext();
        var (map, _, _) = Build(context, OperationType.Insert);

        Assert.Equal("[we]]ird]", SqlServerSqlBuilder.Quote("we]ird"));
        Assert.Equal("[Item]", SqlServerSqlBuilder.TableName(map));

        var (custom, _, _) = Build(context, OperationType.Insert, o => o.CustomDestinationTableName = "archive.ItemCopy");
        Assert.Equal("[archive].[ItemCopy]", SqlServerSqlBuilder.TableName(custom));
    }

    [Fact]
    public void Upsert_merge_has_insert_update_compare_and_output()
    {
        using var context = CreateContext();
        var (map, config, values) = Build(context, OperationType.InsertOrUpdate, o => o.SetOutputIdentity = true);
        var (insert, _) = map.GetInsertColumns(new List<Item> { new() }, values);

        var sql = SqlServerSqlBuilder.Merge(map, OperationType.InsertOrUpdate, config, "[#S]", 3, insert, "[#O]", map.ServerGeneratedColumns);

        Assert.StartsWith("MERGE [Item] WITH (HOLDLOCK) AS T USING (SELECT TOP (3) * FROM [#S] ORDER BY [__Index]) AS S ON T.[Id] = S.[Id]", sql, StringComparison.Ordinal);
        Assert.Contains(" WHEN NOT MATCHED BY TARGET THEN INSERT ([", sql, StringComparison.Ordinal);
        Assert.Contains(", [Name], ", sql, StringComparison.Ordinal);
        Assert.Contains("]) VALUES (S.[", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("INSERT ([Id]", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("[Version]) VALUES", sql, StringComparison.Ordinal);
        Assert.Contains(" WHEN MATCHED AND EXISTS (SELECT S.[", sql, StringComparison.Ordinal);
        Assert.Contains(" EXCEPT SELECT T.[", sql, StringComparison.Ordinal);
        Assert.Contains(" THEN UPDATE SET T.[", sql, StringComparison.Ordinal);
        Assert.Contains("T.[Name] = S.[Name]", sql, StringComparison.Ordinal);
        Assert.Contains(" OUTPUT S.[__Index], SUBSTRING($action, 1, 1), INSERTED.[Id], INSERTED.[Version] INTO [#O] ([__Index], [__Action], [Id], [Version]);", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("T.[Id] = S.[Id],", sql, StringComparison.Ordinal);
    }

    [Fact]
    public void WithHoldlock_false_and_PreserveInsertOrder_false_simplify_the_merge()
    {
        using var context = CreateContext();
        var (map, config, _) = Build(context, OperationType.Update, o => { o.WithHoldlock = false; o.PreserveInsertOrder = false; });

        var sql = SqlServerSqlBuilder.Merge(map, OperationType.Update, config, "[#S]", 1, Array.Empty<ColumnMap>(), null, Array.Empty<ColumnMap>());

        Assert.StartsWith("MERGE [Item] AS T USING [#S] AS S ON T.[Id] = S.[Id] WHEN MATCHED", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("HOLDLOCK", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("NOT MATCHED", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("OUTPUT", sql, StringComparison.Ordinal);
    }

    [Fact]
    public void Insert_merge_never_matches_and_delete_merge_only_deletes()
    {
        using var context = CreateContext();
        var (insertMap, insertConfig, _) = Build(context, OperationType.Insert, o => o.SetOutputIdentity = true);
        var insertSql = SqlServerSqlBuilder.Merge(insertMap, OperationType.Insert, insertConfig, "[#S]", 1, insertMap.WriteColumns, "[#O]", insertMap.ServerGeneratedColumns);
        Assert.Contains(" AS S ON 1 = 0 WHEN NOT MATCHED BY TARGET THEN INSERT", insertSql, StringComparison.Ordinal);
        Assert.DoesNotContain("WHEN MATCHED", insertSql, StringComparison.Ordinal);

        var (deleteMap, deleteConfig, _) = Build(context, OperationType.Delete);
        var deleteSql = SqlServerSqlBuilder.Merge(deleteMap, OperationType.Delete, deleteConfig, "[#S]", 1, Array.Empty<ColumnMap>(), null, Array.Empty<ColumnMap>());
        Assert.EndsWith(" ON T.[Id] = S.[Id] WHEN MATCHED THEN DELETE;", deleteSql, StringComparison.Ordinal);
    }

    [Fact]
    public void Sync_merge_deletes_rows_missing_from_the_source()
    {
        using var context = CreateContext();
        var (map, config, _) = Build(context, OperationType.InsertOrUpdateOrDelete);
        var sql = SqlServerSqlBuilder.Merge(map, OperationType.InsertOrUpdateOrDelete, config, "[#S]", 0, map.WriteColumns, null, Array.Empty<ColumnMap>());

        Assert.Contains(" WHEN NOT MATCHED BY SOURCE THEN DELETE;", sql, StringComparison.Ordinal);
    }

    [Fact]
    public void Nothing_to_update_with_output_uses_a_variable_no_op_instead_of_touching_the_key()
    {
        using var context = CreateContext();
        var (map, config, _) = Build(context, OperationType.InsertOrUpdate, o =>
        {
            o.SetOutputIdentity = true;
            o.PropertiesToInclude = new List<string> { nameof(Item.Id) };
        });
        Assert.Empty(map.UpdateColumns);

        var sql = SqlServerSqlBuilder.Merge(map, OperationType.InsertOrUpdate, config, "[#S]", 1, Array.Empty<ColumnMap>(), "[#O]", map.ServerGeneratedColumns);

        Assert.StartsWith("DECLARE @tsd_noop int; MERGE", sql, StringComparison.Ordinal);
        Assert.Contains(" WHEN MATCHED THEN UPDATE SET @tsd_noop = 1 OUTPUT", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("T.[Id] = T.[Id]", sql, StringComparison.Ordinal);
    }

    [Fact]
    public void OnConflictUpdateWhereSql_is_appended_to_the_update_branch()
    {
        using var context = CreateContext();
        var (map, config, _) = Build(context, OperationType.Update, o => o.OnConflictUpdateWhereSql = (t, s) => $"{s}.[TimeUpdated] > {t}.[TimeUpdated]");
        var sql = SqlServerSqlBuilder.Merge(map, OperationType.Update, config, "[#S]", 1, Array.Empty<ColumnMap>(), null, Array.Empty<ColumnMap>());

        Assert.Contains(" AND (S.[TimeUpdated] > T.[TimeUpdated]) THEN UPDATE SET", sql, StringComparison.Ordinal);
    }

    [Fact]
    public void Staging_table_has_a_clustered_index_column_and_rowversion_as_varbinary()
    {
        using var context = CreateContext();
        var (map, _, _) = Build(context, OperationType.Update);
        var sql = SqlServerSqlBuilder.CreateStagingTable("[#S]", map.WriteColumns);
        var output = SqlServerSqlBuilder.CreateOutputTable("[#O]", map.ServerGeneratedColumns);

        Assert.StartsWith("CREATE TABLE [#S] ([Id] bigint NULL, ", sql, StringComparison.Ordinal);
        Assert.EndsWith("[__Index] int NOT NULL PRIMARY KEY CLUSTERED);", sql, StringComparison.Ordinal);
        Assert.DoesNotContain("[Version]", sql, StringComparison.Ordinal);
        Assert.Contains("[Version] varbinary(8) NULL", output, StringComparison.Ordinal);
        Assert.StartsWith("CREATE TABLE [#O] ([__Index] int NULL, [__Action] char(1) NOT NULL, [Id] bigint NULL", output, StringComparison.Ordinal);
        Assert.Equal("IF OBJECT_ID('tempdb..#S') IS NOT NULL DROP TABLE [#S]; IF OBJECT_ID('tempdb..#O') IS NOT NULL DROP TABLE [#O];", SqlServerSqlBuilder.DropTempTables(new[] { "[#S]", "[#O]" }));
    }

    [Fact]
    public void Read_select_has_the_output_shape()
    {
        using var context = CreateContext();
        var (map, _, _) = Build(context, OperationType.Read, o => o.PropertiesToInclude = new List<string> { nameof(Item.Name) });

        var sql = SqlServerSqlBuilder.SelectRead(map, "[#S]", map.ReadColumns);

        Assert.Equal("SELECT S.[__Index], 'R' AS [__Action], T.[Id], T.[Name] FROM [Item] AS T INNER JOIN [#S] AS S ON T.[Id] = S.[Id];", sql);
    }
}
