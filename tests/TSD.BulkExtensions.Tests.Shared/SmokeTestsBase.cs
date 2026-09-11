using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests;

/// <summary>
/// Provider-independent tests. Each provider test project derives once and supplies a context factory.
/// </summary>
/// <typeparam name="TAdapter">The adapter type expected to be resolved for the provider under test.</typeparam>
public abstract class SmokeTestsBase<TAdapter> where TAdapter : IBulkAdapter
{
    protected abstract TestDbContext CreateContext();

    private static List<Item> OneItem() => new() { new Item { Name = "x", TimeUpdated = DateTime.UtcNow } };

    [Fact]
    public async Task Database_is_reachable_and_schema_exists()
    {
        var name = $"smoke-{Guid.NewGuid():N}";

        await using (var context = CreateContext())
        {
            context.Items.Add(new Item { Name = name, Quantity = 1, Price = 9.99m, TimeUpdated = DateTime.UtcNow });
            await context.SaveChangesAsync();
        }

        await using (var context = CreateContext())
        {
            var count = await context.Items.CountAsync(x => x.Name == name);
            Assert.Equal(1, count);
        }
    }

    [Fact]
    public async Task Empty_list_is_a_no_op_and_never_reaches_the_adapter()
    {
        await using var context = CreateContext();

        // Adapters are not implemented yet in P0; an empty list must short-circuit before dispatch.
        await context.BulkInsertAsync(new List<Item>());
        await context.BulkUpdateAsync(new List<Item>());
        await context.BulkDeleteAsync(new List<Item>());
        context.BulkInsertOrUpdate(new List<Item>());
    }

    [Fact]
    public void Adapter_resolves_from_provider_name()
    {
        using var context = CreateContext();

        var adapter = BulkAdapterRegistry.Resolve(context);

        Assert.IsType<TAdapter>(adapter);
        Assert.EndsWith(adapter.ProviderNameSuffix, context.Database.ProviderName!, StringComparison.OrdinalIgnoreCase);
        Assert.Same(adapter, BulkAdapterRegistry.Resolve(context));
    }

    [Fact]
    public async Task Conflicting_config_is_rejected_before_dispatch()
    {
        await using var context = CreateContext();

        var ex = await Assert.ThrowsAsync<InvalidBulkConfigException>(() =>
            context.BulkUpdateAsync(OneItem(), opt =>
            {
                opt.PropertiesToInclude = new List<string> { nameof(Item.Name) };
                opt.PropertiesToExclude = new List<string> { nameof(Item.Price) };
            }));

        Assert.Contains(nameof(BulkConfig.PropertiesToInclude), ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Conflicting_config_is_rejected_even_for_an_empty_list()
    {
        await using var context = CreateContext();

        await Assert.ThrowsAsync<InvalidBulkConfigException>(() =>
            context.BulkUpdateAsync(new List<Item>(), opt => opt.BatchSize = -1));
    }

    [Fact]
    public async Task Async_failures_surface_through_the_task_not_the_call()
    {
        await using var context = CreateContext();

        // Must not throw here: a drop-in for the async upstream API faults the Task instead.
        var task = context.BulkUpdateAsync(OneItem(), opt => opt.BatchSize = -1);

        await Assert.ThrowsAsync<InvalidBulkConfigException>(() => task);
    }

    [Fact]
    public async Task Caller_config_is_not_mutated()
    {
        await using var context = CreateContext();
        var config = new BulkConfig { IncludeGraph = true, SetOutputIdentity = false, EnableShadowProperties = false };

        await context.BulkInsertAsync(new List<Item>(), config);

        Assert.False(config.SetOutputIdentity);
        Assert.False(config.EnableShadowProperties);
    }

    [Fact]
    public async Task UseTempDB_outside_a_transaction_is_rejected()
    {
        await using var context = CreateContext();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            context.BulkInsertOrUpdateAsync(OneItem(), opt => opt.UseTempDB = true));
    }

    [Fact]
    public void IncludeGraph_implies_output_identity_and_insert_order()
    {
        var config = new BulkConfig { IncludeGraph = true, PreserveInsertOrder = false, SetOutputIdentity = false };

        config.Prepare(OperationType.Insert);

        Assert.True(config.SetOutputIdentity);
        Assert.True(config.PreserveInsertOrder);
        Assert.True(config.EnableShadowProperties);
    }

    [Fact]
    public void SetOutputIdentity_alone_leaves_PreserveInsertOrder_untouched()
    {
        var config = new BulkConfig { SetOutputIdentity = true, PreserveInsertOrder = false };

        config.Prepare(OperationType.Insert);

        Assert.False(config.PreserveInsertOrder);
    }

    [Fact]
    public void IncludeGraph_rejects_delete()
    {
        var config = new BulkConfig { IncludeGraph = true };

        Assert.Throws<InvalidBulkConfigException>(() => config.Prepare(OperationType.Delete));
    }
}
