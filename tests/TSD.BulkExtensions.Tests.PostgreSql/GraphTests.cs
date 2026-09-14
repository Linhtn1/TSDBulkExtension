using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.PostgreSql;

[Collection(PostgreSqlCollection.Name)]
public sealed class GraphTests : GraphTestsBase
{
    private readonly PostgreSqlFixture _fixture;

    public GraphTests(PostgreSqlFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
