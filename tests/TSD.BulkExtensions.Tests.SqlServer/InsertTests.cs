using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

/// <summary>Fast-path BulkInsert (no SetOutputIdentity) through SqlBulkCopy.</summary>
public sealed class InsertTests : SqlServerTestBase
{
    public InsertTests(SqlServerFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Inserts_rows_with_converted_values_and_nulls()
    {
        var tag = Tag("ins");
        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(Items(tag, 10));
        }

        var rows = await LoadAsync(tag);
        Assert.Equal(10, rows.Count);
        Assert.All(rows, r => Assert.True(r.Id > 0));
        Assert.Null(rows[2].Description);
        Assert.Equal("desc 1", rows[0].Description);
        Assert.Equal(ItemStatus.Active, rows[1].Status);
        Assert.Equal(ItemStatus.Draft, rows[0].Status);
        Assert.Equal(20m, rows[9].Price);
        Assert.NotNull(rows[0].Version);
    }

    [Fact]
    public void Sync_overload_inserts_too()
    {
        var tag = Tag("ins");
        using (var context = CreateContext())
        {
            context.BulkInsert(Items(tag, 5));
        }

        using (var context = CreateContext())
        {
            Assert.Equal(5, context.Items.Count(x => x.Name.StartsWith(tag)));
        }
    }

    [Fact]
    public async Task Large_list_streams_in_batches_and_reports_progress()
    {
        var tag = Tag("ins");
        var reported = new List<decimal>();

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(Items(tag, 10_000), opt => { opt.BatchSize = 1000; opt.NotifyAfter = 1000; }, progress: reported.Add);
        }

        await using (var context = CreateContext())
        {
            Assert.Equal(10_000, await context.Items.CountAsync(x => x.Name.StartsWith(tag)));
        }

        Assert.True(reported.Count >= 10, $"progress reported {reported.Count} times");
        Assert.Equal(1m, reported[^1]);
        Assert.Equal(reported.OrderBy(x => x), reported);
    }

    [Fact]
    public async Task Server_generated_guid_key_is_filled_when_left_empty()
    {
        var tag = Tag("ins");
        var owner = (await SeedAsync(tag, 1))[0];

        await using (var context = CreateContext())
        {
            var histories = Enumerable.Range(1, 3).Select(i => new ItemHistory { ItemId = owner.Id, Remark = $"{tag}-{i}", CreatedAt = DateTime.UtcNow }).ToList();
            await context.BulkInsertAsync(histories);
        }

        await using (var context = CreateContext())
        {
            var rows = await context.ItemHistories.Where(x => x.ItemId == owner.Id).ToListAsync();
            Assert.Equal(3, rows.Count);
            Assert.All(rows, r => Assert.NotEqual(Guid.Empty, r.Id));
            Assert.Equal(3, rows.Select(r => r.Id).Distinct().Count());
        }
    }

    [Fact]
    public async Task Preset_guid_key_is_kept()
    {
        var tag = Tag("ins");
        var owner = (await SeedAsync(tag, 1))[0];
        var presetId = Guid.NewGuid();

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(new List<ItemHistory> { new() { Id = presetId, ItemId = owner.Id, Remark = tag, CreatedAt = DateTime.UtcNow } });
        }

        await using (var context = CreateContext())
        {
            Assert.NotNull(await context.ItemHistories.FindAsync(presetId));
        }
    }

    [Fact]
    public async Task KeepIdentity_preserves_explicit_ids()
    {
        var tag = Tag("ins");
        var baseId = 5_000_000L + Random.Shared.Next(1_000_000);
        var items = Items(tag, 3);
        for (var i = 0; i < items.Count; i++)
        {
            items[i].Id = baseId + i;
        }

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(items, opt => opt.SqlBulkCopyOptions = SqlBulkCopyOptions.KeepIdentity);
        }

        var ids = (await LoadAsync(tag)).Select(x => x.Id).OrderBy(x => x).ToList();
        Assert.Equal(new[] { baseId, baseId + 1, baseId + 2 }, ids);
    }

    [Fact]
    public async Task Joins_the_ambient_transaction()
    {
        var tag = Tag("ins");
        await using (var context = CreateContext())
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.BulkInsertAsync(Items(tag, 4));

            Assert.Equal(4, await context.Items.CountAsync(x => x.Name.StartsWith(tag)));
            await transaction.RollbackAsync();
        }

        Assert.Empty(await LoadAsync(tag));
    }

    [Fact]
    public async Task Cancelled_token_aborts_before_writing()
    {
        var tag = Tag("ins");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await using (var context = CreateContext())
        {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => context.BulkInsertAsync(Items(tag, 100), cancellationToken: cts.Token));
        }

        Assert.Empty(await LoadAsync(tag));
    }

    [Fact]
    public async Task Connection_is_released_after_the_operation()
    {
        await using var context = CreateContext();
        var connection = context.Database.GetDbConnection();

        await context.BulkInsertAsync(Items(Tag("ins"), 2));

        Assert.Equal(System.Data.ConnectionState.Closed, connection.State);
    }

    [Fact]
    public async Task Tph_base_typed_list_writes_derived_columns_and_discriminator()
    {
        var reference = Tag("tph");
        var payments = new List<Payment>
        {
            new CardPayment { Reference = reference, Amount = 10, CardLast4 = "4242" },
            new CashPayment { Reference = reference, Amount = 20, Register = "R1" },
        };

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(payments);
        }

        await using (var context = CreateContext())
        {
            var stored = await context.Payments.Where(p => p.Reference == reference).OrderBy(p => p.Amount).ToListAsync();
            var card = Assert.IsType<CardPayment>(stored[0]);
            var cash = Assert.IsType<CashPayment>(stored[1]);
            Assert.Equal("4242", card.CardLast4);
            Assert.Equal("R1", cash.Register);
        }
    }
}
