using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests;

/// <summary>IncludeGraph: parents before children, generated keys propagated into foreign keys.</summary>
public abstract class GraphTestsBase : ProviderTestBase
{
    private static Order NewOrder(string code, int lines) => new()
    {
        Code = code,
        OrderDate = DateTime.UtcNow,
        Total = lines * 10m,
        Lines = Enumerable.Range(1, lines).Select(i => new OrderLine { ItemId = i, Quantity = i, UnitPrice = 10m }).ToList(),
    };

    [Fact]
    public async Task Insert_one_order_with_lines_sets_keys_and_foreign_keys()
    {
        var tag = Tag("grf");
        var order = NewOrder(tag, 3);

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(new List<Order> { order }, opt => opt.IncludeGraph = true);
        }

        Assert.True(order.Id > 0);
        Assert.NotEqual(default, order.CreatedAt); // server default read back alongside the key
        Assert.All(order.Lines, l => Assert.Equal(order.Id, l.OrderId));
        Assert.All(order.Lines, l => Assert.True(l.Id > 0));

        await using (var context = CreateContext())
        {
            var stored = await context.Orders.Include(o => o.Lines).SingleAsync(o => o.Code == tag);
            Assert.Equal(3, stored.Lines.Count);
            Assert.Equal(order.CreatedAt, stored.CreatedAt);
            Assert.Equal(new[] { 1, 2, 3 }, stored.Lines.OrderBy(l => l.Quantity).Select(l => l.Quantity));
        }
    }

    [Fact]
    public async Task Insert_many_orders_writes_every_child_under_its_own_parent()
    {
        var tag = Tag("grf");
        var orders = Enumerable.Range(1, 50).Select(i => NewOrder($"{tag}-{i}", 3)).ToList();

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(orders, opt => opt.IncludeGraph = true);
        }

        await using (var context = CreateContext())
        {
            var stored = await context.Orders.Include(o => o.Lines).Where(o => o.Code.StartsWith(tag)).ToListAsync();
            Assert.Equal(50, stored.Count);
            Assert.All(stored, o => Assert.Equal(3, o.Lines.Count));
        }

        foreach (var order in orders)
        {
            Assert.All(order.Lines, l => Assert.Equal(order.Id, l.OrderId));
        }
    }

    [Fact]
    public async Task Reference_navigation_from_child_saves_the_parent_first()
    {
        var tag = Tag("grf");
        var order = new Order { Code = tag, OrderDate = DateTime.UtcNow };
        var lines = new List<OrderLine>
        {
            new() { Order = order, ItemId = 1, Quantity = 1, UnitPrice = 1m },
            new() { Order = order, ItemId = 2, Quantity = 2, UnitPrice = 2m },
        };

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(lines, opt => opt.IncludeGraph = true);
        }

        Assert.True(order.Id > 0);
        Assert.All(lines, l => Assert.Equal(order.Id, l.OrderId));

        await using (var context = CreateContext())
        {
            Assert.Equal(2, await context.OrderLines.CountAsync(l => l.OrderId == order.Id));
        }
    }

    [Fact]
    public async Task Shared_parent_instance_is_written_once()
    {
        var tag = Tag("grf");
        var item = new Item { Name = tag, TimeUpdated = DateTime.UtcNow };
        var histories = Enumerable.Range(1, 3).Select(i => new ItemHistory { Item = item, Remark = $"{tag}-{i}", CreatedAt = DateTime.UtcNow }).ToList();

        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(histories, opt => opt.IncludeGraph = true);
        }

        Assert.True(item.Id > 0);
        Assert.All(histories, h => Assert.Equal(item.Id, h.ItemId));
        Assert.All(histories, h => Assert.NotEqual(Guid.Empty, h.Id));

        await using (var context = CreateContext())
        {
            Assert.Equal(1, await context.Items.CountAsync(i => i.Name == tag));
            Assert.Equal(3, await context.ItemHistories.CountAsync(h => h.ItemId == item.Id));
        }
    }

    [Fact]
    public async Task Upsert_graph_updates_parent_adds_new_children_and_aggregates_stats()
    {
        var tag = Tag("grf");
        var order = NewOrder(tag, 2);
        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(new List<Order> { order }, opt => opt.IncludeGraph = true);
        }

        order.Total = 999m;
        order.Lines.First().Quantity = 50;
        order.Lines.Add(new OrderLine { ItemId = 9, Quantity = 9, UnitPrice = 9m });
        var config = new BulkConfig { IncludeGraph = true, CalculateStats = true };

        await using (var context = CreateContext())
        {
            await context.BulkInsertOrUpdateAsync(new List<Order> { order }, config);
        }

        Assert.NotNull(config.StatsInfo);
        Assert.Equal(1, config.StatsInfo!.StatsNumberInserted);
        Assert.Equal(2, config.StatsInfo.StatsNumberUpdated);

        await using (var context = CreateContext())
        {
            var stored = await context.Orders.Include(o => o.Lines).SingleAsync(o => o.Code == tag);
            Assert.Equal(999m, stored.Total);
            Assert.Equal(3, stored.Lines.Count);
            Assert.Contains(stored.Lines, l => l.Quantity == 50);
            Assert.Contains(stored.Lines, l => l.ItemId == 9 && l.OrderId == order.Id);
        }
    }

    [Fact]
    public async Task Update_graph_persists_changes_on_both_levels()
    {
        var tag = Tag("grf");
        var order = NewOrder(tag, 2);
        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(new List<Order> { order }, opt => opt.IncludeGraph = true);
        }

        order.Total = 123m;
        foreach (var line in order.Lines)
        {
            line.UnitPrice = 77m;
        }

        await using (var context = CreateContext())
        {
            await context.BulkUpdateAsync(new List<Order> { order }, opt => opt.IncludeGraph = true);
        }

        await using (var context = CreateContext())
        {
            var stored = await context.Orders.Include(o => o.Lines).SingleAsync(o => o.Code == tag);
            Assert.Equal(123m, stored.Total);
            Assert.All(stored.Lines, l => Assert.Equal(77m, l.UnitPrice));
        }
    }

    [Fact]
    public async Task Include_list_naming_a_parent_only_property_is_tolerated_on_children()
    {
        var tag = Tag("grf");
        var order = NewOrder(tag, 2);
        await using (var context = CreateContext())
        {
            await context.BulkInsertAsync(new List<Order> { order }, opt => opt.IncludeGraph = true);
        }

        order.Total = 555m;
        order.Code = "should-not-persist";
        foreach (var line in order.Lines)
        {
            line.UnitPrice = 1m;
        }

        await using (var context = CreateContext())
        {
            // OrderLine has no Total: the child pass resolves to nothing to update and is skipped, not rejected.
            await context.BulkUpdateAsync(new List<Order> { order }, opt =>
            {
                opt.IncludeGraph = true;
                opt.PropertiesToInclude = new List<string> { nameof(Order.Total) };
            });
        }

        await using (var context = CreateContext())
        {
            var stored = await context.Orders.Include(o => o.Lines).SingleAsync(o => o.Code == tag);
            Assert.Equal(555m, stored.Total);
            Assert.All(stored.Lines, l => Assert.Equal(10m, l.UnitPrice));
        }
    }

    [Fact]
    public async Task Sync_with_IncludeGraph_is_rejected()
    {
        await using var context = CreateContext();

        await Assert.ThrowsAsync<InvalidBulkConfigException>(() =>
            context.BulkInsertOrUpdateOrDeleteAsync(new List<Order> { NewOrder(Tag("grf"), 1) }, opt => opt.IncludeGraph = true));
    }

    [Fact]
    public async Task Failure_in_a_pass_rolls_back_the_whole_graph()
    {
        var tag = Tag("grf");
        var orders = new List<Order> { NewOrder(tag, 1), NewOrder(tag, 1) }; // duplicate Code violates the unique index

        await using (var context = CreateContext())
        {
            await Assert.ThrowsAnyAsync<DbException>(() => context.BulkInsertAsync(orders, opt => opt.IncludeGraph = true));
        }

        await using (var context = CreateContext())
        {
            Assert.Equal(0, await context.Orders.CountAsync(o => o.Code == tag));
            Assert.Equal(System.Data.ConnectionState.Closed, context.Database.GetDbConnection().State);
        }
    }

    [Fact]
    public async Task Graph_joins_an_ambient_transaction_and_rolls_back_with_it()
    {
        var tag = Tag("grf");
        var order = NewOrder(tag, 2);

        await using (var context = CreateContext())
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            await context.BulkInsertAsync(new List<Order> { order }, opt => opt.IncludeGraph = true);
            Assert.True(order.Id > 0);
            await transaction.RollbackAsync();
        }

        await using (var context = CreateContext())
        {
            Assert.Equal(0, await context.Orders.CountAsync(o => o.Code == tag));
        }
    }
}
