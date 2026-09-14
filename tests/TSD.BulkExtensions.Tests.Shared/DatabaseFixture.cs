using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests;

/// <summary>
/// One database container per test run with the test schema created once.
/// Provider test projects supply the container and the <c>Use*()</c> call.
/// </summary>
public abstract class DatabaseFixture<TContainer> : IAsyncLifetime
    where TContainer : DockerContainer, IDatabaseContainer
{
    private readonly TContainer _container;

    protected DatabaseFixture(TContainer container) => _container = container;

    public string ConnectionString => _container.GetConnectionString();

    protected abstract void Configure(DbContextOptionsBuilder<TestDbContext> builder, string connectionString);

    public TestDbContext CreateContext()
    {
        var builder = new DbContextOptionsBuilder<TestDbContext>();
        Configure(builder, ConnectionString);
        return new TestDbContext(builder.Options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
}
