using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

[Collection(SqlServerCollection.Name)]
public sealed class SmokeTests : SmokeTestsBase<BulkExtensions.SqlServer.SqlServerBulkAdapter>
{
    private readonly SqlServerFixture _fixture;

    public SmokeTests(SqlServerFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
