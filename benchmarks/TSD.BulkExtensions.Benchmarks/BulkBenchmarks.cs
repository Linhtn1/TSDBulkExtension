using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TSD.BulkExtensions.Tests.Model;

namespace TSD.BulkExtensions.Benchmarks;

/// <summary>
/// TSD.BulkExtensions against the last MIT release of EFCore.BulkExtensions (6.5.6) and plain SaveChanges, on SQL Server.
/// Set TSD_BENCH_SQLSERVER to a connection string to use an existing server; otherwise a container is started.
/// Both libraries are invoked through their static classes so the identically named extension methods never clash.
/// </summary>
[Config(typeof(BenchConfig))]
[MemoryDiagnoser]
public class BulkBenchmarks
{
    private MsSqlContainer? _container;
    private string _connectionString = string.Empty;
    private List<Item> _fresh = new();
    private List<Item> _existing = new();

    [Params(10_000, 50_000)]
    public int N { get; set; }

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _connectionString = Environment.GetEnvironmentVariable("TSD_BENCH_SQLSERVER") ?? string.Empty;
        if (_connectionString.Length == 0)
        {
            _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();
            await _container.StartAsync();
            _connectionString = _container.GetConnectionString();
        }

        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    [IterationSetup]
    public void IterationSetup()
    {
        using var context = CreateContext();
        context.Database.ExecuteSqlRaw("DELETE FROM [ItemHistory]; DELETE FROM [Item];");

        _fresh = NewItems(N);
        _existing = NewItems(N);
        TSD.BulkExtensions.DbContextBulkExtensions.BulkInsert(context, _existing, new BulkConfig { SetOutputIdentity = true });
        foreach (var item in _existing)
        {
            item.Quantity += 1;
            item.Status = ItemStatus.Active;
        }
    }

    // ---- Insert -----------------------------------------------------------------------------------------------------

    [Benchmark(Baseline = true, Description = "Insert: TSD")]
    public void Insert_Tsd()
    {
        using var context = CreateContext();
        TSD.BulkExtensions.DbContextBulkExtensions.BulkInsert(context, _fresh);
    }

    [Benchmark(Description = "Insert: EFCore.BulkExtensions 6.5.6")]
    public void Insert_Upstream()
    {
        using var context = CreateContext();
        EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsert(context, _fresh);
    }

    [Benchmark(Description = "Insert: SaveChanges")]
    public void Insert_SaveChanges()
    {
        if (N > 10_000)
        {
            return; // SaveChanges at 100K takes minutes; keep the run bounded
        }

        using var context = CreateContext();
        context.Items.AddRange(_fresh);
        context.SaveChanges();
    }

    // ---- Insert with generated keys --------------------------------------------------------------------------------

    [Benchmark(Description = "Insert+Ids: TSD")]
    public void InsertWithIds_Tsd()
    {
        using var context = CreateContext();
        TSD.BulkExtensions.DbContextBulkExtensions.BulkInsert(context, _fresh, new BulkConfig { SetOutputIdentity = true });
    }

    [Benchmark(Description = "Insert+Ids: EFCore.BulkExtensions 6.5.6")]
    public void InsertWithIds_Upstream()
    {
        using var context = CreateContext();
        EFCore.BulkExtensions.DbContextBulkExtensions.BulkInsert(context, _fresh, new EFCore.BulkExtensions.BulkConfig { SetOutputIdentity = true });
    }

    // ---- Update -----------------------------------------------------------------------------------------------------

    [Benchmark(Description = "Update: TSD")]
    public void Update_Tsd()
    {
        using var context = CreateContext();
        TSD.BulkExtensions.DbContextBulkExtensions.BulkUpdate(context, _existing);
    }

    [Benchmark(Description = "Update: EFCore.BulkExtensions 6.5.6")]
    public void Update_Upstream()
    {
        using var context = CreateContext();
        EFCore.BulkExtensions.DbContextBulkExtensions.BulkUpdate(context, _existing);
    }

    // ---- helpers ----------------------------------------------------------------------------------------------------

    private TestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>().UseSqlServer(_connectionString).Options;
        return new TestDbContext(options);
    }

    private static List<Item> NewItems(int count) => Enumerable.Range(1, count)
        .Select(i => new Item
        {
            Name = $"bench-{i}",
            Description = i % 5 == 0 ? null : $"description {i}",
            Quantity = i % 100,
            Price = (i % 1000) * 1.25m,
            Status = (ItemStatus)(i % 3),
            TimeUpdated = DateTime.UtcNow,
        })
        .ToList();
}
