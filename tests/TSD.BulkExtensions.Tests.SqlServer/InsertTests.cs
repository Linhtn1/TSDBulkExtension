using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

[Collection(SqlServerCollection.Name)]
public sealed class InsertTests : InsertTestsBase
{
    private readonly SqlServerFixture _fixture;

    public InsertTests(SqlServerFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
