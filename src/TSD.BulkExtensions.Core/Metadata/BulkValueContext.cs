using Microsoft.EntityFrameworkCore;

namespace TSD.BulkExtensions.Metadata;

/// <summary>
/// Per-operation services a <see cref="ColumnMap"/> needs to read or write a value: the context (for shadow properties and
/// the model) and an optional shadow-value resolver. Kept out of the map itself so maps stay cacheable across operations.
/// </summary>
public sealed class BulkValueContext
{
    /// <summary>Creates the context for one operation.</summary>
    public BulkValueContext(DbContext context, Func<object, string, object?>? shadowValue = null)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        ShadowValue = shadowValue;
    }

    /// <summary>The context the operation runs on.</summary>
    public DbContext Context { get; }

    /// <summary>
    /// Supplies shadow property values (entity, property name) instead of reading them through <c>DbContext.Entry</c>.
    /// Comes from <see cref="BulkConfig.ShadowPropertyValue"/> or from the graph executor propagating generated keys.
    /// </summary>
    public Func<object, string, object?>? ShadowValue { get; }
}
