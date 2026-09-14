using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

[Collection(SqlServerCollection.Name)]
public sealed class MetadataTests : MetadataTestsBase
{
    private readonly SqlServerFixture _fixture;

    public MetadataTests(SqlServerFixture fixture) => _fixture = fixture;

    protected override TestDbContext CreateContext() => _fixture.CreateContext();
}
