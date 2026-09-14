using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Graph;

namespace TSD.BulkExtensions;

/// <summary>
/// Single entry point behind every public extension method: validates input and options, short-circuits empty lists,
/// resolves the provider adapter and dispatches. The caller's <see cref="BulkConfig"/> is never mutated; only
/// <see cref="BulkConfig.StatsInfo"/> is written back after the operation.
/// </summary>
internal static class BulkExecutor
{
    public static void Execute<T>(DbContext context, Type? type, IList<T> entities, OperationType operationType, BulkConfig? config, Action<decimal>? progress)
        where T : class
    {
        var operation = Prepare(context, type, entities, operationType, config, progress);
        if (operation is null)
        {
            return;
        }

        var adapter = BulkAdapterRegistry.Resolve(context);
        if (operation.Config.IncludeGraph)
        {
            GraphExecutor.Execute(operation, adapter);
        }
        else
        {
            adapter.Execute(operation);
        }

        PublishResults(operation, config);
    }

    public static async Task ExecuteAsync<T>(DbContext context, Type? type, IList<T> entities, OperationType operationType, BulkConfig? config, Action<decimal>? progress, CancellationToken cancellationToken)
        where T : class
    {
        var operation = Prepare(context, type, entities, operationType, config, progress);
        if (operation is null)
        {
            return;
        }

        var adapter = BulkAdapterRegistry.Resolve(context);
        if (operation.Config.IncludeGraph)
        {
            await GraphExecutor.ExecuteAsync(operation, adapter, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            await adapter.ExecuteAsync(operation, cancellationToken).ConfigureAwait(false);
        }

        PublishResults(operation, config);
    }

    /// <summary>
    /// Validates arguments and options first, so a bad call fails the same way whether or not the list is empty.
    /// Returns <c>null</c> when there is nothing to do (empty list and the operation is not table-wide).
    /// </summary>
    private static BulkOperation<T>? Prepare<T>(DbContext context, Type? type, IList<T> entities, OperationType operationType, BulkConfig? config, Action<decimal>? progress)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(entities);

        var effective = (config ?? new BulkConfig()).Clone();
        effective.Prepare(operationType);

        // Sync and Truncate act on the whole table and are meaningful with an empty list.
        var tableWide = operationType is OperationType.InsertOrUpdateOrDelete or OperationType.Truncate;
        if (entities.Count == 0 && !tableWide)
        {
            return null;
        }

        return new BulkOperation<T>(context, type ?? typeof(T), entities, operationType, effective, progress);
    }

    private static void PublishResults<T>(BulkOperation<T> operation, BulkConfig? callerConfig) where T : class
    {
        if (callerConfig is not null && operation.Config.StatsInfo is not null)
        {
            callerConfig.StatsInfo = operation.Config.StatsInfo;
        }
    }

    public static BulkConfig ToConfig(Action<BulkConfig>? configure)
    {
        var config = new BulkConfig();
        configure?.Invoke(config);
        return config;
    }
}
