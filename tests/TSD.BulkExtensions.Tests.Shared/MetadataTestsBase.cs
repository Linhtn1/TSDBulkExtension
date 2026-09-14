using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Streaming;
using TSD.BulkExtensions.Tests.Model;
using TSD.BulkExtensions.Transactions;
using Xunit;

namespace TSD.BulkExtensions.Tests;

/// <summary>
/// EntityTableMap, EntityDataReader and ConnectionScope against the real provider model.
/// </summary>
public abstract class MetadataTestsBase
{
    protected abstract TestDbContext CreateContext();

    /// <summary>Builds a map the way an adapter does: prepared config, provider identity detection.</summary>
    public static EntityTableMap Map(TestDbContext context, Type type, OperationType op, Action<BulkConfig>? configure = null)
        => ProviderTestBase.BuildMap(context, type, op, configure).Map;

    private static BulkValueContext Values(DbContext context) => new(context);

    private static string[] Paths(IEnumerable<ColumnMap> columns) => columns.Select(c => c.PropertyPath).OrderBy(x => x, StringComparer.Ordinal).ToArray();

    // ---- EntityTableMap ---------------------------------------------------------------------------------------------

    [Fact]
    public void Item_map_describes_table_keys_and_identity()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.Insert);

        Assert.Equal("Item", map.TableName);
        Assert.Null(map.Schema);
        Assert.Equal(new[] { nameof(Item.Id) }, Paths(map.PrimaryKeyColumns));
        Assert.Same(map.PrimaryKeyColumns[0], map.MatchColumns[0]);
        Assert.NotNull(map.IdentityColumn);
        Assert.Equal(nameof(Item.Id), map.IdentityColumn!.PropertyPath);

        var expected = new List<string> { "Description", "Id", "IsDeleted", "Name", "Price", "Quantity", "Status", "TimeUpdated" };
        if (context.IsSqlServer)
        {
            expected.Add("Version");
        }
        Assert.Equal(expected.OrderBy(x => x, StringComparer.Ordinal), Paths(map.Columns));
    }

    [Fact]
    public void Insert_write_columns_exclude_identity_and_rowversion()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.Insert);

        Assert.DoesNotContain(map.WriteColumns, c => c.IsIdentity);
        Assert.DoesNotContain(map.WriteColumns, c => c.IsRowVersion);
        Assert.Contains(map.WriteColumns, c => c.PropertyPath == nameof(Item.Name));

        if (context.IsSqlServer)
        {
            Assert.NotNull(map.RowVersionColumn);
            Assert.Contains(map.ServerGeneratedColumns, c => c.IsRowVersion);
        }
        Assert.Contains(map.ServerGeneratedColumns, c => c.IsIdentity);
    }

    [Fact]
    public void Update_keeps_identity_in_staging_but_out_of_the_insert_branch()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.InsertOrUpdate);
        var (insert, leftToServer) = map.GetInsertColumns(new List<Item> { new() { Name = "x" } }, Values(context));

        Assert.Contains(map.WriteColumns, c => c.IsIdentity);
        Assert.DoesNotContain(insert, c => c.IsIdentity);
        Assert.Contains(leftToServer, c => c.IsIdentity);
        Assert.DoesNotContain(map.UpdateColumns, c => c.IsIdentity);
    }

    [Fact]
    public void KeepIdentity_puts_the_identity_column_back_into_write_columns()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.Insert, o => o.SqlBulkCopyOptions = Microsoft.Data.SqlClient.SqlBulkCopyOptions.KeepIdentity);

        Assert.True(map.KeepIdentity);
        Assert.Contains(map.WriteColumns, c => c.IsIdentity);
    }

    [Fact]
    public void Converted_enum_column_uses_provider_type_both_ways()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.Insert);
        var status = map.FindColumn(nameof(Item.Status))!;
        var item = new Item { Status = ItemStatus.Active };

        Assert.Equal(typeof(string), status.ProviderClrType);
        Assert.NotNull(status.Converter);
        Assert.Equal("Active", status.GetProviderValue(item, Values(context)));

        status.SetFromProviderValue(item, Values(context), "Archived");
        Assert.Equal(ItemStatus.Archived, item.Status);
    }

    [Fact]
    public void Guid_default_key_is_left_to_server_only_when_every_entity_leaves_it_empty()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(ItemHistory), OperationType.Insert);
        var id = map.FindColumn(nameof(ItemHistory.Id))!;

        Assert.True(id.HasServerDefault);
        Assert.False(id.IsIdentity);
        Assert.Contains(id, map.DefaultValueColumns);
        Assert.Contains(id, map.ServerGeneratedColumns);

        var allEmpty = new List<ItemHistory> { new() { Remark = "a" }, new() { Remark = "b" } };
        var (insert, leftToServer) = map.GetInsertColumns(allEmpty, Values(context));
        Assert.DoesNotContain(id, insert);
        Assert.Contains(id, leftToServer);

        var preset = new List<ItemHistory> { new() { Id = Guid.NewGuid(), Remark = "a" }, new() { Remark = "b" } };
        (insert, leftToServer) = map.GetInsertColumns(preset, Values(context));
        Assert.Contains(id, insert);
        Assert.Empty(leftToServer);
    }

    [Fact]
    public void PropertiesToInclude_narrows_write_compare_update_and_read_but_keeps_keys()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.Update, o => o.PropertiesToInclude = new List<string> { nameof(Item.Name) });

        Assert.Equal(new[] { "Id", "Name" }, Paths(map.WriteColumns));
        Assert.Equal(new[] { "Name" }, Paths(map.CompareColumns));
        Assert.Equal(new[] { "Name" }, Paths(map.UpdateColumns));

        var read = Map(context, typeof(Item), OperationType.Read, o => o.PropertiesToInclude = new List<string> { nameof(Item.Name) });
        Assert.Equal(new[] { "Id" }, Paths(read.WriteColumns));
        Assert.Equal(new[] { "Id", "Name" }, Paths(read.ReadColumns));
    }

    [Fact]
    public void PropertiesToExclude_drops_columns_but_keeps_keys()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.InsertOrUpdate, o => o.PropertiesToExclude = new List<string> { nameof(Item.Description), nameof(Item.Id) });

        Assert.DoesNotContain(map.WriteColumns, c => c.PropertyPath == nameof(Item.Description));
        Assert.Contains(map.WriteColumns, c => c.PropertyPath == nameof(Item.Name));
        Assert.DoesNotContain(map.UpdateColumns, c => c.IsPrimaryKey);
    }

    [Fact]
    public void Unknown_property_name_is_rejected_with_its_name_unless_told_to_ignore()
    {
        using var context = CreateContext();

        var ex = Assert.Throws<InvalidBulkConfigException>(() =>
            Map(context, typeof(Item), OperationType.Update, o => o.PropertiesToInclude = new List<string> { "Nmae" }));

        Assert.Contains("Nmae", ex.Message, StringComparison.Ordinal);
        Assert.Contains(nameof(BulkConfig.PropertiesToInclude), ex.Message, StringComparison.Ordinal);

        var lenient = Map(context, typeof(Item), OperationType.Update, o =>
        {
            o.PropertiesToInclude = new List<string> { "Nmae", nameof(Item.Name) };
            o.IgnoreUnknownPropertyNames = true;
        });
        Assert.Equal(new[] { "Name" }, Paths(lenient.UpdateColumns));
    }

    [Fact]
    public void UpdateByProperties_replaces_the_match_key_and_is_never_updated()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.InsertOrUpdate, o => o.UpdateByProperties = new List<string> { nameof(Item.Name) });

        Assert.Equal(new[] { "Name" }, Paths(map.MatchColumns));
        Assert.DoesNotContain(map.UpdateColumns, c => c.PropertyPath == nameof(Item.Name));
        Assert.Contains(map.UpdateColumns, c => c.PropertyPath == nameof(Item.Quantity));
    }

    [Fact]
    public void Delete_and_read_only_carry_the_match_key()
    {
        using var context = CreateContext();

        Assert.Equal(new[] { "Id" }, Paths(Map(context, typeof(Item), OperationType.Delete).WriteColumns));
        Assert.Equal(new[] { "Id" }, Paths(Map(context, typeof(Item), OperationType.Read).WriteColumns));
    }

    [Fact]
    public void Custom_destination_table_overrides_schema_and_name()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Item), OperationType.Insert, o => o.CustomDestinationTableName = "archive.ItemCopy");

        Assert.Equal("archive", map.Schema);
        Assert.Equal("ItemCopy", map.TableName);
    }

    [Fact]
    public void Maps_are_cached_per_model_and_options_including_shadow_resolver_users()
    {
        using var context = CreateContext();

        var a = Map(context, typeof(Item), OperationType.Insert);
        var b = Map(context, typeof(Item), OperationType.Insert);
        var c = Map(context, typeof(Item), OperationType.Insert, o => o.PropertiesToExclude = new List<string> { nameof(Item.Description) });
        var d = Map(context, typeof(Item), OperationType.Insert, o => o.ShadowPropertyValue = (_, _) => 1);

        Assert.Same(a, b);
        Assert.NotSame(a, c);
        Assert.Same(a, d);
    }

    [Fact]
    public void Tph_base_typed_map_carries_derived_columns_discriminator_and_private_key_setter()
    {
        using var context = CreateContext();

        var map = Map(context, typeof(Payment), OperationType.Insert, o => o.SetOutputIdentity = true);
        var values = Values(context);

        Assert.NotNull(map.DiscriminatorColumn);
        Assert.Equal(new[] { "Amount", "CardLast4", "Id", "Kind", "Reference", "Register" }, Paths(map.Columns));

        var card = new CardPayment { Reference = "c", Amount = 1, CardLast4 = "4242" };
        var cash = new CashPayment { Reference = "k", Amount = 2, Register = "R1" };
        var last4 = map.FindColumn(nameof(CardPayment.CardLast4))!;
        Assert.Equal("4242", last4.GetProviderValue(card, values));
        Assert.Null(last4.GetProviderValue(cash, values));
        Assert.Equal("card", map.DiscriminatorColumn!.GetProviderValue(card, values));
        Assert.Equal("cash", map.DiscriminatorColumn.GetProviderValue(cash, values));

        var id = map.FindColumn(nameof(Payment.Id))!;
        Assert.True(id.CanWriteBack);
        Assert.Contains(id, map.ServerGeneratedColumns);
        id.SetFromProviderValue(card, values, 42L);
        Assert.Equal(42L, card.Id);
    }

    // ---- EntityDataReader -------------------------------------------------------------------------------------------

    [Fact]
    public void Reader_streams_rows_with_provider_values_and_index()
    {
        using var context = CreateContext();
        var map = Map(context, typeof(Item), OperationType.Insert);
        var items = new List<Item>
        {
            new() { Name = "a", Quantity = 1, Price = 1.5m, Status = ItemStatus.Active, TimeUpdated = DateTime.UtcNow },
            new() { Name = "b", Description = null, Quantity = 2, Price = 2.5m, TimeUpdated = DateTime.UtcNow },
            new() { Name = "c", Description = "desc", Quantity = 3, Price = 3.5m, TimeUpdated = DateTime.UtcNow },
        };

        using var reader = new EntityDataReader<Item>(items, Values(context), map.WriteColumns, includeIndexColumn: true);

        Assert.Equal(map.WriteColumns.Count + 1, reader.FieldCount);
        Assert.Equal(EntityDataReader<Item>.IndexColumnName, reader.GetName(reader.FieldCount - 1));
        Assert.Equal(typeof(int), reader.GetFieldType(reader.FieldCount - 1));

        var name = reader.GetOrdinal("Name");
        var status = reader.GetOrdinal("Status");
        var description = reader.GetOrdinal("Description");
        Assert.Equal(typeof(string), reader.GetFieldType(status));

        Assert.True(reader.Read());
        Assert.Equal("a", reader.GetString(name));
        Assert.Equal("Active", reader.GetValue(status));
        Assert.Equal(0, reader.GetInt32(reader.FieldCount - 1));

        Assert.True(reader.Read());
        Assert.True(reader.IsDBNull(description));
        Assert.Equal(DBNull.Value, reader.GetValue(description));
        Assert.Equal("Draft", reader.GetValue(status));
        Assert.Equal(1, reader.CurrentIndex);

        Assert.True(reader.Read());
        Assert.Equal("desc", reader.GetValue(description));
        Assert.Equal(2, reader.GetValue(reader.FieldCount - 1));

        Assert.False(reader.Read());
    }

    [Fact]
    public void Reader_rejects_access_before_first_read()
    {
        using var context = CreateContext();
        var map = Map(context, typeof(Item), OperationType.Insert);

        using var reader = new EntityDataReader<Item>(new List<Item>(), Values(context), map.WriteColumns, includeIndexColumn: false);

        Assert.False(reader.HasRows);
        Assert.Throws<InvalidOperationException>(() => reader.GetValue(0));
        Assert.False(reader.Read());
    }

    [Fact]
    public void Reader_schema_table_lists_every_field()
    {
        using var context = CreateContext();
        var map = Map(context, typeof(Item), OperationType.Insert);

        using var reader = new EntityDataReader<Item>(new List<Item>(), Values(context), map.WriteColumns, includeIndexColumn: true);
        var schema = reader.GetSchemaTable();

        Assert.Equal(reader.FieldCount, schema.Rows.Count);
        Assert.Equal("Name", schema.Rows[reader.GetOrdinal("Name")]["ColumnName"]);
        Assert.Equal(typeof(int), schema.Rows[reader.FieldCount - 1]["DataType"]);
    }

    // ---- ConnectionScope --------------------------------------------------------------------------------------------

    [Fact]
    public async Task Scope_opens_and_closes_the_connection_when_nobody_else_holds_it()
    {
        await using var context = CreateContext();
        var connection = context.Database.GetDbConnection();
        Assert.Equal(ConnectionState.Closed, connection.State);

        await using (var scope = await ConnectionScope.OpenAsync(context, CancellationToken.None))
        {
            Assert.Equal(ConnectionState.Open, scope.Connection.State);
            Assert.False(scope.HasTransaction);
            Assert.Null(scope.Transaction);

            await using var command = scope.CreateCommand("SELECT 1");
            Assert.Equal(1, Convert.ToInt32(await command.ExecuteScalarAsync(), System.Globalization.CultureInfo.InvariantCulture));
        }

        Assert.Equal(ConnectionState.Closed, connection.State);
    }

    [Fact]
    public async Task Scope_joins_an_existing_transaction_and_leaves_the_connection_open()
    {
        await using var context = CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var connection = context.Database.GetDbConnection();

        await using (var scope = await ConnectionScope.OpenAsync(context, CancellationToken.None))
        {
            Assert.True(scope.HasTransaction);
            Assert.Same(transaction.GetDbTransaction(), scope.Transaction);

            await using var command = scope.CreateCommand("SELECT 1");
            Assert.Same(scope.Transaction, command.Transaction);
            await command.ExecuteScalarAsync();
        }

        Assert.Equal(ConnectionState.Open, connection.State);
        await transaction.RollbackAsync();
    }
}
