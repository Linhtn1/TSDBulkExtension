using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Output;
using TSD.BulkExtensions.Streaming;
using TSD.BulkExtensions.Transactions;

namespace TSD.BulkExtensions.SqlServer;

/// <summary>
/// The staged path: bulk-copy the entities into a session temp table, run one MERGE (or SELECT for reads) against it,
/// read generated values back through an OUTPUT table, drop the temp tables. Everything runs on the context's
/// connection and joins the caller's transaction; without one, <see cref="OperationUnit"/> opens its own so the
/// statement and the write-back succeed or fail together.
/// </summary>
internal static class SqlServerMergeExecutor
{
    public static async Task ExecuteAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var config = operation.Config;
        var entities = operation.Entities;
        var operationType = operation.OperationType;

        if (map.IsNoOp)
        {
            return;
        }

        var suffix = Guid.NewGuid().ToString("N")[..8];
        var staging = SqlServerSqlBuilder.StagingTableName(map, suffix);
        var stagingColumns = map.WriteColumns;
        var (insertColumns, _) = map.GetInsertColumns(entities, operation.Values);

        var wantsOutput = config.SetOutputIdentity || config.CalculateStats;
        var outputColumns = config.SetOutputIdentity && operationType != OperationType.Delete ? map.ServerGeneratedColumns : Array.Empty<ColumnMap>();
        var output = wantsOutput ? SqlServerSqlBuilder.OutputTableName(map, suffix) : null;
        var identityInsert = map.KeepIdentity && map.IdentityColumn is not null && insertColumns.Contains(map.IdentityColumn);

        var createTables = SqlServerSqlBuilder.CreateStagingTable(staging, stagingColumns)
            + (output is null ? string.Empty : " " + SqlServerSqlBuilder.CreateOutputTable(output, outputColumns));
        var merge = SqlServerSqlBuilder.Merge(map, operationType, config, staging, entities.Count, insertColumns, output, outputColumns);

        await using var unit = await OperationUnit.BeginAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await unit.Scope.ExecuteNonQueryAsync(createTables, isAsync, cancellationToken).ConfigureAwait(false);
            await CopyToStagingAsync(operation, unit.Scope, staging, stagingColumns, isAsync, cancellationToken).ConfigureAwait(false);

            if (identityInsert)
            {
                await unit.Scope.ExecuteNonQueryAsync(SqlServerSqlBuilder.IdentityInsert(map, on: true), isAsync, cancellationToken).ConfigureAwait(false);
            }

            try
            {
                await unit.Scope.ExecuteNonQueryAsync(merge, isAsync, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                if (identityInsert)
                {
                    await unit.Scope.ExecuteQuietlyAsync(SqlServerSqlBuilder.IdentityInsert(map, on: false), isAsync).ConfigureAwait(false);
                }
            }

            if (output is not null)
            {
                var rows = await unit.Scope.ReadOutputRowsAsync(SqlServerSqlBuilder.SelectOutput(output, outputColumns), outputColumns.Count, isAsync, cancellationToken).ConfigureAwait(false);
                var stats = OutputWriteBack.Apply(entities, operation.Values, outputColumns, rows);
                if (config.CalculateStats)
                {
                    config.StatsInfo = stats;
                }
            }

            await unit.CommitAsync(cancellationToken).ConfigureAwait(false);
            operation.Progress?.Invoke(1m);
        }
        finally
        {
            await unit.Scope.ExecuteQuietlyAsync(SqlServerSqlBuilder.DropTempTables(output is null ? new[] { staging } : new[] { staging, output }), isAsync).ConfigureAwait(false);
        }
    }

    public static async Task ReadAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var staging = SqlServerSqlBuilder.StagingTableName(map, suffix);
        var readColumns = map.ReadColumns;

        await using var unit = await OperationUnit.BeginAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await unit.Scope.ExecuteNonQueryAsync(SqlServerSqlBuilder.CreateStagingTable(staging, map.WriteColumns), isAsync, cancellationToken).ConfigureAwait(false);
            await CopyToStagingAsync(operation, unit.Scope, staging, map.WriteColumns, isAsync, cancellationToken).ConfigureAwait(false);

            var rows = await unit.Scope.ReadOutputRowsAsync(SqlServerSqlBuilder.SelectRead(map, staging, readColumns), readColumns.Count, isAsync, cancellationToken).ConfigureAwait(false);
            OutputWriteBack.Apply(operation.Entities, operation.Values, readColumns, rows);

            await unit.CommitAsync(cancellationToken).ConfigureAwait(false);
            operation.Progress?.Invoke(1m);
        }
        finally
        {
            await unit.Scope.ExecuteQuietlyAsync(SqlServerSqlBuilder.DropTempTables(new[] { staging }), isAsync).ConfigureAwait(false);
        }
    }

    public static async Task TruncateAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var scope = await ConnectionScope.OpenAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await scope.ExecuteNonQueryAsync(SqlServerSqlBuilder.Truncate(map), isAsync, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await scope.DisposeAsync(isAsync).ConfigureAwait(false);
        }
    }

    // ---- helpers -----------------------------------------------------------------------------------------------------

    private static async Task CopyToStagingAsync<T>(BulkOperation<T> operation, ConnectionScope scope, string staging, IReadOnlyList<ColumnMap> columns, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        using var bulkCopy = SqlServerBulkAdapter.CreateBulkCopy(scope, operation.Config, staging, columns, operation.Entities.Count, operation.Progress, includeIndexColumn: true);
        using var reader = new EntityDataReader<T>(operation.Entities, operation.Values, columns, includeIndexColumn: true);

        if (isAsync)
        {
            await bulkCopy.WriteToServerAsync(reader, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            bulkCopy.WriteToServer(reader);
        }
    }
}
