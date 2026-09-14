using Microsoft.EntityFrameworkCore;

namespace TSD.BulkExtensions;

/// <summary>
/// Bulk operations on a <see cref="DbContext"/>. Signatures are identical to EFCore.BulkExtensions.
/// </summary>
public static class DbContextBulkExtensions
{
    // ---- Insert ----------------------------------------------------------------------------------------------------

    /// <summary>Inserts all entities.</summary>
    public static void BulkInsert<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, entities, OperationType.Insert, bulkConfig, progress);

    /// <summary>Inserts all entities.</summary>
    public static Task BulkInsertAsync<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, entities, OperationType.Insert, bulkConfig, progress, cancellationToken);

    /// <summary>Inserts all entities.</summary>
    public static void BulkInsert<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkInsert(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type);

    /// <summary>Inserts all entities.</summary>
    public static Task BulkInsertAsync<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkInsertAsync(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type, cancellationToken);

    // ---- InsertOrUpdate --------------------------------------------------------------------------------------------

    /// <summary>Inserts entities that do not exist and updates the ones that do (upsert).</summary>
    public static void BulkInsertOrUpdate<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, entities, OperationType.InsertOrUpdate, bulkConfig, progress);

    /// <summary>Inserts entities that do not exist and updates the ones that do (upsert).</summary>
    public static Task BulkInsertOrUpdateAsync<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, entities, OperationType.InsertOrUpdate, bulkConfig, progress, cancellationToken);

    /// <summary>Inserts entities that do not exist and updates the ones that do (upsert).</summary>
    public static void BulkInsertOrUpdate<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkInsertOrUpdate(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type);

    /// <summary>Inserts entities that do not exist and updates the ones that do (upsert).</summary>
    public static Task BulkInsertOrUpdateAsync<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkInsertOrUpdateAsync(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type, cancellationToken);

    // ---- InsertOrUpdateOrDelete (sync) -----------------------------------------------------------------------------

    /// <summary>
    /// Makes the whole table match the list: inserts missing rows, updates matched ones and <b>deletes every other row of the table</b>.
    /// There is no filter; an empty list empties the table. Entities with duplicate keys in the list make the statement fail.
    /// </summary>
    public static void BulkInsertOrUpdateOrDelete<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, entities, OperationType.InsertOrUpdateOrDelete, bulkConfig, progress);

    /// <summary>
    /// Makes the whole table match the list: inserts missing rows, updates matched ones and <b>deletes every other row of the table</b>.
    /// There is no filter; an empty list empties the table. Entities with duplicate keys in the list make the statement fail.
    /// </summary>
    public static Task BulkInsertOrUpdateOrDeleteAsync<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, entities, OperationType.InsertOrUpdateOrDelete, bulkConfig, progress, cancellationToken);

    /// <summary>
    /// Makes the whole table match the list: inserts missing rows, updates matched ones and <b>deletes every other row of the table</b>.
    /// There is no filter; an empty list empties the table. Entities with duplicate keys in the list make the statement fail.
    /// </summary>
    public static void BulkInsertOrUpdateOrDelete<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkInsertOrUpdateOrDelete(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type);

    /// <summary>
    /// Makes the whole table match the list: inserts missing rows, updates matched ones and <b>deletes every other row of the table</b>.
    /// There is no filter; an empty list empties the table. Entities with duplicate keys in the list make the statement fail.
    /// </summary>
    public static Task BulkInsertOrUpdateOrDeleteAsync<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkInsertOrUpdateOrDeleteAsync(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type, cancellationToken);

    // ---- Update ----------------------------------------------------------------------------------------------------

    /// <summary>Updates all entities by key.</summary>
    public static void BulkUpdate<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, entities, OperationType.Update, bulkConfig, progress);

    /// <summary>Updates all entities by key.</summary>
    public static Task BulkUpdateAsync<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, entities, OperationType.Update, bulkConfig, progress, cancellationToken);

    /// <summary>Updates all entities by key.</summary>
    public static void BulkUpdate<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkUpdate(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type);

    /// <summary>Updates all entities by key.</summary>
    public static Task BulkUpdateAsync<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkUpdateAsync(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type, cancellationToken);

    // ---- Delete ----------------------------------------------------------------------------------------------------

    /// <summary>Deletes all entities by key.</summary>
    public static void BulkDelete<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, entities, OperationType.Delete, bulkConfig, progress);

    /// <summary>Deletes all entities by key.</summary>
    public static Task BulkDeleteAsync<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, entities, OperationType.Delete, bulkConfig, progress, cancellationToken);

    /// <summary>Deletes all entities by key.</summary>
    public static void BulkDelete<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkDelete(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type);

    /// <summary>Deletes all entities by key.</summary>
    public static Task BulkDeleteAsync<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkDeleteAsync(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type, cancellationToken);

    // ---- Read ------------------------------------------------------------------------------------------------------

    /// <summary>Loads current database values into the given entities, matched by key (or <see cref="BulkConfig.UpdateByProperties"/>).</summary>
    public static void BulkRead<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, entities, OperationType.Read, bulkConfig, progress);

    /// <summary>Loads current database values into the given entities, matched by key (or <see cref="BulkConfig.UpdateByProperties"/>).</summary>
    public static Task BulkReadAsync<T>(this DbContext context, IList<T> entities, BulkConfig? bulkConfig = null, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, entities, OperationType.Read, bulkConfig, progress, cancellationToken);

    /// <summary>Loads current database values into the given entities, matched by key (or <see cref="BulkConfig.UpdateByProperties"/>).</summary>
    public static void BulkRead<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null) where T : class
        => BulkRead(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type);

    /// <summary>Loads current database values into the given entities, matched by key (or <see cref="BulkConfig.UpdateByProperties"/>).</summary>
    public static Task BulkReadAsync<T>(this DbContext context, IList<T> entities, Action<BulkConfig>? bulkAction, Action<decimal>? progress = null, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkReadAsync(context, entities, BulkExecutor.ToConfig(bulkAction), progress, type, cancellationToken);

    // ---- Truncate --------------------------------------------------------------------------------------------------

    /// <summary>Truncates the table the entity type is mapped to.</summary>
    public static void Truncate<T>(this DbContext context, Type? type = null) where T : class
        => BulkExecutor.Execute(context, type, Array.Empty<T>(), OperationType.Truncate, null, null);

    /// <summary>Truncates the table the entity type is mapped to.</summary>
    public static Task TruncateAsync<T>(this DbContext context, Type? type = null, CancellationToken cancellationToken = default) where T : class
        => BulkExecutor.ExecuteAsync(context, type, Array.Empty<T>(), OperationType.Truncate, null, null, cancellationToken);
}
