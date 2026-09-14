using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.PostgreSql;

[Collection(PostgreSqlCollection.Name)]
public sealed class InsertTests : InsertTestsBase
{
    private readonly PostgreSqlFixture _fixture;

    public InsertTests(PostgreSqlFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
