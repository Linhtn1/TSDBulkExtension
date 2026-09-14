using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

/// <summary>Shared fixture access and test-data helpers for the SQL Server integration tests.</summary>
[Collection(SqlServerCollection.Name)]
public abstract class SqlServerTestBase
{
    protected SqlServerTestBase(SqlServerFixture fixture) => Fixture = fixture;

    protected SqlServerFixture Fixture { get; }

    protected TestDbContext CreateContext() => Fixture.CreateContext();

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

    /// <summary>Inserts items through EF (not the library under test) and returns them with keys and rowversions.</summary>
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
