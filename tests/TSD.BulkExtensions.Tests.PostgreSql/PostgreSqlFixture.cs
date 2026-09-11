using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.PostgreSql;

public sealed class PostgreSqlFixture : DatabaseFixture<PostgreSqlContainer>
{
    public PostgreSqlFixture()
        : base(new PostgreSqlBuilder("postgres:16-alpine").Build())
    {
    }

    protected override void Configure(DbContextOptionsBuilder<TestDbContext> builder, string connectionString)
        => builder.UseNpgsql(connectionString);
}

[CollectionDefinition(Name)]
public sealed class PostgreSqlCollection : ICollectionFixture<PostgreSqlFixture>
{
    public const string Name = "PostgreSql";
}
