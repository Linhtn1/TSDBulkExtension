using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

[Collection(SqlServerCollection.Name)]
public sealed class MergeTests : MergeTestsBase
{
    private readonly SqlServerFixture _fixture;

    public MergeTests(SqlServerFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
