using Microsoft.EntityFrameworkCore;

namespace TSD.BulkExtensions.Tests.Model;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemHistory> ItemHistories => Set<ItemHistory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    private bool IsSqlServer => Database.ProviderName?.EndsWith("SqlServer", StringComparison.OrdinalIgnoreCase) == true;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(b =>
        {
            b.ToTable("Item");
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(1000);
            b.Property(x => x.Price).HasPrecision(18, 2);
            b.HasQueryFilter(x => !x.IsDeleted);
        });

        modelBuilder.Entity<ItemHistory>(b =>
        {
            b.ToTable("ItemHistory");
            b.Property(x => x.Id).HasDefaultValueSql(IsSqlServer ? "newsequentialid()" : "gen_random_uuid()");
            b.Property(x => x.Remark).HasMaxLength(500).IsRequired();
            b.HasOne(x => x.Item).WithMany(x => x.Histories).HasForeignKey(x => x.ItemId);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.ToTable("Order");
            b.Property(x => x.Code).HasMaxLength(50).IsRequired();
            b.Property(x => x.Total).HasPrecision(18, 2);
            b.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<OrderLine>(b =>
        {
            b.ToTable("OrderLine");
            b.Property(x => x.UnitPrice).HasPrecision(18, 2);
            b.HasOne(x => x.Order).WithMany(x => x.Lines).HasForeignKey(x => x.OrderId);
        });
    }
}
