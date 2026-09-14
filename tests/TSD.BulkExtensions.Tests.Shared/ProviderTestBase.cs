using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Tests.Model;

namespace TSD.BulkExtensions.Tests;

/// <summary>Context factory and test-data helpers shared by every provider's integration tests.</summary>
public abstract class ProviderTestBase
{
    private bool? _supportsRowVersion;

    protected abstract TestDbContext CreateContext();

    /// <summary>Whether the provider model maps <see cref="Item.Version"/> as a server-maintained rowversion (SQL Server only).</summary>
    protected bool SupportsRowVersion
    {
        get
        {
            if (_supportsRowVersion is null)
            {
                using var context = CreateContext();
                _supportsRowVersion = context.Model.FindEntityType(typeof(Item))!.FindProperty(nameof(Item.Version)) is not null;
            }

            return _supportsRowVersion.Value;
        }
    }

    /// <summary>Builds a map the way an adapter does: prepared config, provider identity detection.</summary>
    public static (EntityTableMap Map, BulkConfig Config) BuildMap(TestDbContext context, Type type, OperationType op, Action<BulkConfig>? configure = null)
    {
        var config = new BulkConfig();
        configure?.Invoke(config);
        config.Prepare(op);
        var adapter = BulkAdapterRegistry.Resolve(context);
        return (EntityTableMap.Create(context, type, config, op, adapter.IsIdentityColumn), config);
    }

    /// <summary>Unique prefix so tests sharing the database never see each other's rows.</summary>
    protected static string Tag(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    protected static List<Item> Items(string tag, int count) => Enumerable.Range(1, count)
        .Select(i => new Item
        {
            Name = $"{tag}-{i}",
            Description = i % 3 == 0 ? null : $"desc {i}",
            Quantity = i,
            Price = i * 2m,
            Status = i % 2 == 0 ? ItemStatus.Active : ItemStatus.Draft,
            TimeUpdated = new DateTime(2026, 9, 14, 10, 0, 0, DateTimeKind.Utc).AddMinutes(i),
        })
        .ToList();

    /// <summary>Inserts items through EF (not the library under test) and returns them with keys (and rowversions where supported).</summary>
    protected async Task<List<Item>> SeedAsync(string tag, int count)
    {
        await using var context = CreateContext();
        var items = Items(tag, count);
        context.Items.AddRange(items);
        await context.SaveChangesAsync();
        return items;
    }

    /// <summary>Reads the tagged rows back through EF, ordered by Quantity.</summary>
    protected async Task<List<Item>> LoadAsync(string tag)
    {
        await using var context = CreateContext();
        return await context.Items.Where(x => x.Name.StartsWith(tag)).OrderBy(x => x.Quantity).ToListAsync();
    }
}
