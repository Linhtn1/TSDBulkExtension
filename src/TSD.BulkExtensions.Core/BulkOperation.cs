using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Metadata;

namespace TSD.BulkExtensions;

/// <summary>
/// Everything a provider adapter needs to execute one bulk operation.
/// </summary>
/// <typeparam name="T">Static element type of the entity list. May be a base type of the actual entities; see <see cref="EntityType"/>.</typeparam>
public sealed class BulkOperation<T> where T : class
{
    /// <summary>Creates an operation description. Public so custom <see cref="IBulkAdapter"/> implementations can be tested directly.</summary>
    public BulkOperation(DbContext context, Type entityType, IList<T> entities, OperationType operationType, BulkConfig config, Action<decimal>? progress)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(entityType);
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(config);

        Context = context;
        EntityType = entityType;
        Entities = entities;
        OperationType = operationType;
        Config = config;
        Progress = progress;
        Values = new BulkValueContext(context, config.ShadowPropertyValue);
    }

    /// <summary>Per-operation value services for <see cref="ColumnMap"/>: the context and the shadow-value resolver.</summary>
    public BulkValueContext Values { get; }

    /// <summary>The context whose connection and transaction the operation runs on.</summary>
    public DbContext Context { get; }

    /// <summary>
    /// CLR type of the entities as mapped in the EF model. Defaults to <typeparamref name="T"/>; callers pass an explicit type
    /// when the list is declared as a base class or interface.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>The entities to write or read. Order matters when <see cref="BulkConfig.PreserveInsertOrder"/> is set.</summary>
    public IList<T> Entities { get; }

    /// <summary>What to do.</summary>
    public OperationType OperationType { get; }

    /// <summary>Effective options for this operation: a validated, normalised copy of what the caller passed.</summary>
    public BulkConfig Config { get; }

    /// <summary>Optional progress callback receiving a value in the range 0..1.</summary>
    public Action<decimal>? Progress { get; }
}
