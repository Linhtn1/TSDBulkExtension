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
    public DbSet<SyncRow> SyncRows => Set<SyncRow>();
    public DbSet<Payment> Payments => Set<Payment>();

    public bool IsSqlServer => Database.ProviderName?.EndsWith("SqlServer", StringComparison.OrdinalIgnoreCase) == true;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(b =>
        {
            b.ToTable("Item");
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(1000);
            b.Property(x => x.Price).HasPrecision(18, 2);
            b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.HasQueryFilter(x => !x.IsDeleted);

            if (IsSqlServer)
            {
                b.Property(x => x.Version).IsRowVersion();
            }
            else
            {
                b.Ignore(x => x.Version);
            }
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
            b.Property(x => x.CreatedAt).HasDefaultValueSql(IsSqlServer ? "GETUTCDATE()" : "now()");
            b.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<SyncRow>(b =>
        {
            b.ToTable("SyncRow");
            b.Property(x => x.Key).HasMaxLength(50).IsRequired();
            b.Property(x => x.Category).HasMaxLength(50);
        });

        modelBuilder.Entity<Payment>(b =>
        {
            b.ToTable("Payment");
            b.Property(x => x.Reference).HasMaxLength(50).IsRequired();
            b.Property(x => x.Amount).HasPrecision(18, 2);
            b.HasDiscriminator<string>("Kind")
                .HasValue<CardPayment>("card")
                .HasValue<CashPayment>("cash");
        });
        modelBuilder.Entity<CardPayment>(b => b.Property(x => x.CardLast4).HasMaxLength(4));
        modelBuilder.Entity<CashPayment>(b => b.Property(x => x.Register).HasMaxLength(20));

        modelBuilder.Entity<OrderLine>(b =>
        {
            b.ToTable("OrderLine");
            b.Property(x => x.UnitPrice).HasPrecision(18, 2);
            b.HasOne(x => x.Order).WithMany(x => x.Lines).HasForeignKey(x => x.OrderId);
        });
    }
}
