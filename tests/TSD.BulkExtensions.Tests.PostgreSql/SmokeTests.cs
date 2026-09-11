using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.PostgreSql;

[Collection(PostgreSqlCollection.Name)]
public sealed class SmokeTests : SmokeTestsBase<BulkExtensions.PostgreSql.PostgreSqlBulkAdapter>
{
    private readonly PostgreSqlFixture _fixture;

    public SmokeTests(PostgreSqlFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
