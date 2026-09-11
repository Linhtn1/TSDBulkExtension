using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TSD.BulkExtensions.Tests.Model;
using Xunit;

namespace TSD.BulkExtensions.Tests.SqlServer;

public sealed class SqlServerFixture : DatabaseFixture<MsSqlContainer>
{
    public SqlServerFixture()
        : base(new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build())
    {
    }

    protected override void Configure(DbContextOptionsBuilder<TestDbContext> builder, string connectionString)
        => builder.UseSqlServer(connectionString);
}

[CollectionDefinition(Name)]
public sealed class SqlServerCollection : ICollectionFixture<SqlServerFixture>
{
    public const string Name = "SqlServer";
}
