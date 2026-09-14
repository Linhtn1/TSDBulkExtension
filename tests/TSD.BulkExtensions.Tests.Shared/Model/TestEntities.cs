namespace TSD.BulkExtensions.Tests.Model;

public enum ItemStatus
{
    Draft = 0,
    Active = 1,
    Archived = 2,
}

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

    /// <summary>Stored as a string through a value converter.</summary>
    public ItemStatus Status { get; set; }

    /// <summary>Rowversion on SQL Server; ignored in the PostgreSQL model.</summary>
    public byte[]? Version { get; set; }

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

/// <summary>Small table owned by the sync/truncate tests, which affect every row of it.</summary>
public class SyncRow
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public int Value { get; set; }

    /// <summary>Nullable, non-key column used as a custom match key in the null-safe matching tests.</summary>
    public string? Category { get; set; }
}

/// <summary>TPH base with a private key setter: exercises derived-only columns on a base-typed list and write-back through a non-public setter.</summary>
public abstract class Payment
{
    public long Id { get; private set; }
    public string Reference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class CardPayment : Payment
{
    public string CardLast4 { get; set; } = string.Empty;
}

public class CashPayment : Payment
{
    public string Register { get; set; } = string.Empty;
}

/// <summary>Parent of a two-level graph for IncludeGraph tests.</summary>
public class Order
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }

    /// <summary>Server default (GETUTCDATE() / now()); left unset by the tests so the default has to apply.</summary>
    public DateTime CreatedAt { get; set; }

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
