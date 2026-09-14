using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

/// <summary>IQueryable BatchDelete / BatchUpdate: ExecuteDelete/ExecuteUpdate on EF 8+, a compile error on EF 6.</summary>
public sealed class BatchTests : SqlServerTestBase
{
    public BatchTests(SqlServerFixture fixture) : base(fixture)
    {
    }

#if NET8_0_OR_GREATER
    [Fact]
    public async Task BatchDelete_removes_the_selected_rows()
    {
        var tag = Tag("bat");
        await SeedAsync(tag, 5);

        await using var context = CreateContext();
        var affected = await context.Items.Where(x => x.Name.StartsWith(tag) && x.Quantity > 2).BatchDeleteAsync();

        Assert.Equal(3, affected);
        Assert.Equal(new[] { 1, 2 }, (await LoadAsync(tag)).Select(x => x.Quantity));
    }

    [Fact]
    public async Task BatchUpdate_applies_a_member_initialiser_to_the_selected_rows()
    {
        var tag = Tag("bat");
        await SeedAsync(tag, 3);

        await using var context = CreateContext();
        var affected = await context.Items.Where(x => x.Name.StartsWith(tag))
            .BatchUpdateAsync(x => new Item { Quantity = x.Quantity + 10, Status = ItemStatus.Archived, Description = "batched" });

        Assert.Equal(3, affected);
        var rows = await LoadAsync(tag);
        Assert.Equal(new[] { 11, 12, 13 }, rows.Select(r => r.Quantity));
        Assert.All(rows, r => Assert.Equal(ItemStatus.Archived, r.Status));
        Assert.All(rows, r => Assert.Equal("batched", r.Description));
    }

    [Fact]
    public void BatchUpdate_sync_works_and_rejects_non_initialiser_expressions()
    {
        using var context = CreateContext();

        Assert.Throws<ArgumentException>(() => context.Items.Where(x => x.Id < 0).BatchUpdate(x => x));
        Assert.Equal(0, context.Items.Where(x => x.Id < 0).BatchUpdate(x => new Item { Quantity = 0 }));
    }
#else
    [Fact]
    public void Batch_methods_are_compile_errors_on_EF_Core_6()
    {
        foreach (var name in new[] { "BatchDelete", "BatchDeleteAsync", "BatchUpdate", "BatchUpdateAsync" })
        {
            var method = typeof(QueryableBatchExtensions).GetMethod(name, BindingFlags.Public | BindingFlags.Static);
            Assert.NotNull(method);
            var obsolete = method!.GetCustomAttribute<ObsoleteAttribute>();
            Assert.NotNull(obsolete);
            Assert.True(obsolete!.IsError);
            Assert.Contains("ABP EFPlus", obsolete.Message, StringComparison.Ordinal);
        }
    }
#endif
}
