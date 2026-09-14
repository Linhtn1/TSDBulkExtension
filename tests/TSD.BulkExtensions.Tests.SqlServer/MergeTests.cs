using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

/// <summary>Staged operations: temp table + MERGE with OUTPUT.</summary>
public sealed class MergeTests : SqlServerTestBase
{
    public MergeTests(SqlServerFixture fixture) : base(fixture)
    {
    }

    // ---- Insert with output ---------------------------------------------------------------------------------------

    [Fact]
    public async Task Insert_with_SetOutputIdentity_writes_ids_and_rowversion_back_in_list_order()
    {
        var tag = Tag("mrg");
        var items = Items(tag, 50);

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(items, opt => opt.SetOutputIdentity = true);
        }

        Assert.All(items, i => Assert.True(i.Id > 0));
        Assert.All(items, i => Assert.NotNull(i.Version));
        Assert.Equal(items.Select(i => i.Id).OrderBy(x => x), items.Select(i => i.Id));

        var rows = await LoadAsync(tag);
        Assert.Equal(items.Select(i => i.Id), rows.Select(r => r.Id));
    }

    [Fact]
    public async Task Insert_with_output_fills_server_generated_guid_keys()
    {
        var tag = Tag("mrg");
        var owner = (await SeedAsync(tag, 1))[0];
        var histories = Enumerable.Range(1, 3).Select(i => new ItemHistory { ItemId = owner.Id, Remark = $"{tag}-{i}", CreatedAt = DateTime.UtcNow }).ToList();

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(histories, opt => opt.SetOutputIdentity = true);
        }

        Assert.All(histories, h => Assert.NotEqual(Guid.Empty, h.Id));
        await using (var context = CreateContext())
        {
            var stored = await context.ItemHistories.Where(h => h.ItemId == owner.Id).Select(h => h.Id).ToListAsync();
            Assert.Equal(histories.Select(h => h.Id).OrderBy(x => x), stored.OrderBy(x => x));
        }
    }

    [Fact]
    public async Task Insert_with_output_writes_keys_through_a_private_setter_on_a_tph_base()
    {
        var reference = Tag("tph");
        var payments = new List<Payment>
        {
            new CardPayment { Reference = reference, Amount = 1, CardLast4 = "1111" },
            new CashPayment { Reference = reference, Amount = 2, Register = "R2" },
        };

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(payments, opt => opt.SetOutputIdentity = true);
        }

        Assert.All(payments, p => Assert.True(p.Id > 0));
        await using (var context = CreateContext())
        {
            var stored = await context.Payments.Where(p => p.Reference == reference).ToListAsync();
            Assert.Equal(payments.Select(p => p.Id).OrderBy(x => x), stored.Select(p => p.Id).OrderBy(x => x));
        }
    }

    // ---- Update -----------------------------------------------------------------------------------------------------

    [Fact]
    public async Task Update_persists_changed_columns_and_leaves_others()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 5);
        foreach (var item in items)
        {
            item.Price += 100;
            item.Status = ItemStatus.Archived;
        }

        await using (var context = CreateContext())
        {
            await context.BulkUpdateAsync(items);
        }

        var rows = await LoadAsync(tag);
        Assert.All(rows, r => Assert.Equal(ItemStatus.Archived, r.Status));
        Assert.Equal(new[] { 102m, 104m, 106m, 108m, 110m }, rows.Select(r => r.Price));
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, rows.Select(r => r.Quantity));
    }

    [Fact]
    public async Task Update_with_PropertiesToInclude_touches_only_those_columns()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 3);
        foreach (var item in items)
        {
            item.Quantity = 999;
            item.Name = "should-not-persist";
        }

        await using (var context = CreateContext())
        {
            await context.BulkUpdateAsync(items, opt => opt.PropertiesToInclude = new List<string> { nameof(Item.Quantity) });
        }

        var rows = await LoadAsync(tag);
        Assert.Equal(3, rows.Count);
        Assert.All(rows, r => Assert.Equal(999, r.Quantity));
    }

    [Fact]
    public async Task Update_with_identical_values_does_not_bump_rowversion()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 2);
        var before = items.Select(i => i.Version!.ToArray()).ToList();

        await using (var context = CreateContext())
        {
            await context.BulkUpdateAsync(items);
        }

        var rows = await LoadAsync(tag);
        Assert.Equal(before[0], rows[0].Version);
        Assert.Equal(before[1], rows[1].Version);
    }

    [Fact]
    public async Task Update_with_nothing_to_update_is_rejected()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 1);

        await using var context = CreateContext();
        await Assert.ThrowsAsync<InvalidBulkConfigException>(() =>
            context.BulkUpdateAsync(items, opt => opt.PropertiesToInclude = new List<string> { nameof(Item.Id) }));
    }

    // ---- Delete -----------------------------------------------------------------------------------------------------

    [Fact]
    public async Task Delete_removes_matching_rows_and_ignores_missing_ones()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 4);
        var toDelete = new List<Item> { items[0], items[2], new() { Id = long.MaxValue - 5 } };
        var config = new BulkConfig { CalculateStats = true };

        await using (var context = CreateContext())
        {
            await context.BulkDeleteAsync(toDelete, config);
        }

        Assert.Equal(new[] { 2, 4 }, (await LoadAsync(tag)).Select(r => r.Quantity));
        Assert.Equal(2, config.StatsInfo!.StatsNumberDeleted);
    }

    // ---- InsertOrUpdate ---------------------------------------------------------------------------------------------

    [Fact]
    public async Task Upsert_mixes_existing_and_new_rows_and_reports_stats()
    {
        var tag = Tag("mrg");
        var existing = await SeedAsync(tag, 2);
        existing[0].Price = 1000;
        var fresh = Items(tag, 5);
        var batch = new List<Item>(existing) { fresh[2], fresh[3], fresh[4] };
        var config = new BulkConfig { SetOutputIdentity = true, CalculateStats = true };

        await using (var context = CreateContext())
        {
            await context.BulkInsertOrUpdateAsync(batch, config);
        }

        Assert.All(batch, i => Assert.True(i.Id > 0));
        Assert.Equal(existing[0].Id, batch[0].Id);
        Assert.NotNull(config.StatsInfo);
        Assert.Equal(3, config.StatsInfo!.StatsNumberInserted);
        Assert.Equal(1, config.StatsInfo.StatsNumberUpdated);
        Assert.Equal(0, config.StatsInfo.StatsNumberDeleted);

        var rows = await LoadAsync(tag);
        Assert.Equal(5, rows.Count);
        Assert.Equal(1000m, rows[0].Price);
    }

    [Fact]
    public async Task Upsert_by_custom_key_matches_existing_rows_and_returns_their_ids()
    {
        var tag = Tag("mrg");
        var existing = await SeedAsync(tag, 2);
        var incoming = new List<Item>
        {
            new() { Name = existing[0].Name, Quantity = 77, TimeUpdated = DateTime.UtcNow },
            new() { Name = $"{tag}-new", Quantity = 88, TimeUpdated = DateTime.UtcNow },
        };

        await using (var context = CreateContext())
        {
            await context.BulkInsertOrUpdateAsync(incoming, opt =>
            {
                opt.UpdateByProperties = new List<string> { nameof(Item.Name) };
                opt.SetOutputIdentity = true;
            });
        }

        Assert.Equal(existing[0].Id, incoming[0].Id);
        Assert.True(incoming[1].Id > existing[1].Id);

        var rows = await LoadAsync(tag);
        Assert.Equal(3, rows.Count);
        Assert.Equal(77, rows.Single(r => r.Id == existing[0].Id).Quantity);
    }

    [Fact]
    public async Task Upsert_with_nothing_to_update_still_returns_ids_of_matched_rows()
    {
        var tag = Tag("mrg");
        var existing = await SeedAsync(tag, 2);
        var incoming = new List<Item>
        {
            new() { Name = existing[1].Name, TimeUpdated = DateTime.UtcNow },
            new() { Name = $"{tag}-new", Quantity = 9, TimeUpdated = DateTime.UtcNow },
        };

        await using (var context = CreateContext())
        {
            // The update list resolves to the key only, so the UPDATE branch has no columns; matched rows must still reach OUTPUT.
            await context.BulkInsertOrUpdateAsync(incoming, opt =>
            {
                opt.UpdateByProperties = new List<string> { nameof(Item.Name) };
                opt.PropertiesToIncludeOnUpdate = new List<string> { nameof(Item.Id) };
                opt.SetOutputIdentity = true;
            });
        }

        Assert.Equal(existing[1].Id, incoming[0].Id);
        Assert.True(incoming[1].Id > 0);
        Assert.Equal(3, (await LoadAsync(tag)).Count);
    }

    [Fact]
    public async Task Upsert_with_KeepIdentity_inserts_explicit_ids()
    {
        var tag = Tag("mrg");
        var baseId = 7_000_000L + Random.Shared.Next(1_000_000);
        var items = Items(tag, 2);
        items[0].Id = baseId;
        items[1].Id = baseId + 1;

        await using (var context = CreateContext())
        {
            await context.BulkInsertOrUpdateAsync(items, opt => opt.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity);
        }

        Assert.Equal(new[] { baseId, baseId + 1 }, (await LoadAsync(tag)).Select(r => r.Id));
    }

    [Fact]
    public async Task Staged_operations_work_outside_a_transaction_even_with_UseTempDB()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 2);
        items[0].Quantity = 42;

        await using (var context = CreateContext())
        {
            await context.BulkInsertOrUpdateAsync(items, opt => opt.UseTempDB = true);
        }

        Assert.Equal(42, (await LoadAsync(tag)).Single(r => r.Id == items[0].Id).Quantity);
    }

    [Fact]
    public async Task Staged_operations_roll_back_with_the_ambient_transaction()
    {
        var tag = Tag("mrg");
        var items = await SeedAsync(tag, 2);
        items[0].Quantity = 4242;

        await using (var context = CreateContext())
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.BulkUpdateAsync(items);
            await transaction.RollbackAsync();
        }

        Assert.Equal(1, (await LoadAsync(tag))[0].Quantity);
    }

    [Fact]
    public async Task Ambiguous_match_rolls_back_the_merge_when_no_transaction_was_active()
    {
        var tag = Tag("mrg");
        var seeded = await SeedAsync(tag, 2);
        await using (var context = CreateContext())
        {
            // Two existing rows share a Quantity, so matching on Quantity is ambiguous for one incoming row.
            await context.Database.ExecuteSqlRawAsync("UPDATE [Item] SET [Quantity] = 1 WHERE [Id] = {0}", seeded[1].Id);
        }

        var incoming = new List<Item> { new() { Name = $"{tag}-x", Quantity = 1, Price = 500, TimeUpdated = DateTime.UtcNow } };

        await using (var context = CreateContext())
        {
            await Assert.ThrowsAsync<BulkExtensionsException>(() => context.BulkInsertOrUpdateAsync(incoming, opt =>
            {
                opt.UpdateByProperties = new List<string> { nameof(Item.Quantity) };
                opt.SetOutputIdentity = true;
            }));
        }

        // The MERGE itself updated both rows, but the executor's own transaction rolled it back.
        Assert.All(await LoadAsync(tag), r => Assert.NotEqual(500m, r.Price));
    }

    // ---- Sync, Read, Truncate ---------------------------------------------------------------------------------------

    [Fact]
    public async Task Sync_inserts_updates_and_deletes_to_match_the_list()
    {
        await using (var context = CreateContext())
        {
            await context.TruncateAsync<SyncRow>();
            context.SyncRows.AddRange(new SyncRow { Key = "a", Value = 1 }, new SyncRow { Key = "b", Value = 2 }, new SyncRow { Key = "c", Value = 3 });
            await context.SaveChangesAsync();
        }

        var config = new BulkConfig { UpdateByProperties = new List<string> { nameof(SyncRow.Key) }, CalculateStats = true };
        await using (var context = CreateContext())
        {
            await context.BulkInsertOrUpdateOrDeleteAsync(new List<SyncRow>
            {
                new() { Key = "a", Value = 10 },
                new() { Key = "d", Value = 4 },
            }, config);
        }

        await using (var context = CreateContext())
        {
            var rows = await context.SyncRows.OrderBy(r => r.Key).ToListAsync();
            Assert.Equal(new[] { ("a", 10), ("d", 4) }, rows.Select(r => (r.Key, r.Value)));
        }

        Assert.Equal(1, config.StatsInfo!.StatsNumberInserted);
        Assert.Equal(1, config.StatsInfo.StatsNumberUpdated);
        Assert.Equal(2, config.StatsInfo.StatsNumberDeleted);
    }

    [Fact]
    public async Task Read_fills_entities_from_the_table_by_key()
    {
        var tag = Tag("mrg");
        var seeded = await SeedAsync(tag, 3);
        var shells = seeded.Select(s => new Item { Id = s.Id }).ToList();

        await using (var context = CreateContext())
        {
            await context.BulkReadAsync(shells);
        }

        Assert.Equal(seeded.Select(s => s.Name), shells.Select(s => s.Name));
        Assert.Equal(seeded.Select(s => s.Price), shells.Select(s => s.Price));
        Assert.All(shells, s => Assert.NotNull(s.Version));
    }

    [Fact]
    public async Task Read_with_PropertiesToInclude_copies_only_those_columns()
    {
        var tag = Tag("mrg");
        var seeded = await SeedAsync(tag, 2);
        var shells = seeded.Select(s => new Item { Name = s.Name, Price = 12345m }).ToList();

        await using (var context = CreateContext())
        {
            await context.BulkReadAsync(shells, opt =>
            {
                opt.UpdateByProperties = new List<string> { nameof(Item.Name) };
                opt.PropertiesToInclude = new List<string> { nameof(Item.Id) };
            });
        }

        Assert.Equal(seeded.Select(s => s.Id), shells.Select(s => s.Id));
        Assert.All(shells, s => Assert.Equal(12345m, s.Price));
    }

    [Fact]
    public async Task Truncate_empties_the_table()
    {
        await using var context = CreateContext();
        context.SyncRows.Add(new SyncRow { Key = "x", Value = 1 });
        await context.SaveChangesAsync();

        await context.TruncateAsync<SyncRow>();

        Assert.Equal(0, await context.SyncRows.CountAsync());
    }
}
