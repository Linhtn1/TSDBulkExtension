namespace TSD.BulkExtensions.Tests.Model;

/// <summary>Plain entity with a database-generated identity key. The common case in TruePos (ABP Entity&lt;long&gt;).</summary>
public class Item
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime TimeUpdated { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<ItemHistory> Histories { get; set; } = new List<ItemHistory>();
}

/// <summary>Entity with a Guid key that the server fills from a default (newsequentialid / gen_random_uuid). The Acen case.</summary>
public class ItemHistory
{
    public Guid Id { get; set; }
    public long ItemId { get; set; }
    public Item? Item { get; set; }
    public string Remark { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>Parent of a two-level graph for IncludeGraph tests.</summary>
public class Order
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }

    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
}

public class OrderLine
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public Order? Order { get; set; }
    public long ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
