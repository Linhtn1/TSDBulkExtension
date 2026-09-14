using Microsoft.EntityFrameworkCore;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Output;
using TSD.BulkExtensions.Transactions;

namespace TSD.BulkExtensions.PostgreSql;

/// <summary>
/// The staged path on PostgreSQL: COPY the entities into a temp table, assign generated keys there up front
/// (sequence / default expression), run the set statements, then read generated values back by joining on the key.
/// Runs inside the caller's transaction or one of its own.
/// </summary>
internal static class PostgreSqlMergeExecutor
{
    public static async Task ExecuteAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        if (map.IsNoOp)
        {
            return;
        }

        var config = operation.Config;
        var entities = operation.Entities;
        var operationType = operation.OperationType;

        var staging = PostgreSqlSqlBuilder.StagingTableName(map, Guid.NewGuid().ToString("N")[..8]);
        var (insertColumns, leftToServer) = map.GetInsertColumns(entities, operation.Values);
        var inserts = operationType is OperationType.Insert or OperationType.InsertOrUpdate or OperationType.InsertOrUpdateOrDelete;
        var updates = operationType is OperationType.Update or OperationType.InsertOrUpdate or OperationType.InsertOrUpdateOrDelete;
        var nullSafeKeys = NullSafeKeys(map.MatchColumns, entities, operation.Values);

        // With SetOutputIdentity the server-generated key columns are filled in the staging table before the INSERT, so every
        // row carries its key and generated values can be read back by joining on it (or straight from the staging table).
        var insert = new List<ColumnMap>(insertColumns);
        var preAssigned = new List<ColumnMap>();
        var assignSql = new List<string>();
        if (inserts && config.SetOutputIdentity)
        {
            if (map.IdentityColumn is { } identity && !map.KeepIdentity)
            {
                assignSql.Add(PostgreSqlSqlBuilder.AssignIdentity(map, staging, identity, onlyUnmatched: operationType != OperationType.Insert, nullSafeKeys));
                preAssigned.Add(identity);
                insert.Add(identity);
            }

            foreach (var column in leftToServer)
            {
                if (!column.IsIdentity && column.IsPrimaryKey && column.Property.GetDefaultValueSql() is { } defaultSql)
                {
                    assignSql.Add(PostgreSqlSqlBuilder.AssignDefault(staging, column, defaultSql));
                    preAssigned.Add(column);
                    insert.Add(column);
                }
            }
        }

        // Update/Upsert/Sync/Delete stage the write set (the identity is part of it as the match key). A plain Insert stages only
        // what the INSERT selects, which adds the pre-assigned keys and leaves out the columns left to the server, so their CLR
        // defaults (for example DateTime.MinValue for a timestamptz with a default) never reach the provider.
        IReadOnlyList<ColumnMap> stagingColumns = operationType == OperationType.Insert ? insert : map.WriteColumns;

        var identityInserted = map.IdentityColumn is not null && insert.Contains(map.IdentityColumn);
        var overrideSystemValue = identityInserted && PostgreSqlBulkAdapter.IsIdentityAlways(map.IdentityColumn!.Property);
        var orderByIndex = map.IdentityColumn is not null && !identityInserted; // the server assigns keys during the INSERT
        var syncSequence = inserts && identityInserted && map.KeepIdentity;

        var stats = new StatsInfo();
        await using var unit = await OperationUnit.BeginAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        var scope = unit.Scope;
        try
        {
            await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.CreateStagingTable(staging, stagingColumns, onCommitDrop: unit.OwnsTransaction), isAsync, cancellationToken).ConfigureAwait(false);
            await PostgreSqlBulkAdapter.CopyAsync(operation, scope, staging, stagingColumns, includeIndex: true, isAsync, cancellationToken).ConfigureAwait(false);

            foreach (var sql in assignSql)
            {
                await scope.ExecuteNonQueryAsync(sql, isAsync, cancellationToken).ConfigureAwait(false);
            }

            if (operationType == OperationType.InsertOrUpdateOrDelete)
            {
                // Before the INSERT: rows to delete are the table rows the list does not name, and freshly inserted rows whose
                // key the server assigned would otherwise look unnamed too.
                stats.StatsNumberDeleted = await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.DeleteUnmatched(map, staging, nullSafeKeys), isAsync, cancellationToken).ConfigureAwait(false);
            }

            if (updates && map.UpdateColumns.Count > 0)
            {
                stats.StatsNumberUpdated = await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.Update(map, config, staging, nullSafeKeys), isAsync, cancellationToken).ConfigureAwait(false);
            }

            if (inserts)
            {
                var onlyUnmatched = operationType != OperationType.Insert;
                stats.StatsNumberInserted = await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.Insert(map, staging, insert, onlyUnmatched, overrideSystemValue, orderByIndex, nullSafeKeys), isAsync, cancellationToken).ConfigureAwait(false);
            }

            if (operationType == OperationType.Delete)
            {
                stats.StatsNumberDeleted = await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.Delete(map, staging, nullSafeKeys), isAsync, cancellationToken).ConfigureAwait(false);
            }

            if (syncSequence)
            {
                await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.SyncSequence(map, map.IdentityColumn!), isAsync, cancellationToken).ConfigureAwait(false);
            }

            if (config.SetOutputIdentity && operationType != OperationType.Delete && map.ServerGeneratedColumns.Count > 0)
            {
                var generated = map.ServerGeneratedColumns;
                var keys = operationType == OperationType.Insert ? map.PrimaryKeyColumns : map.MatchColumns;
                string select;
                if (operationType == OperationType.Insert && generated.All(preAssigned.Contains))
                {
                    // Every row was assigned its keys in the staging table; nothing to join. Upserts still join so that a
                    // matched row gets the table's key and an ambiguous match is detected.
                    select = PostgreSqlSqlBuilder.SelectStaged(staging, generated);
                }
                else if (keys.Count == 0)
                {
                    throw new InvalidBulkConfigException(
                        $"SetOutputIdentity on PostgreSQL reads generated values back by key, but '{map.EntityType.DisplayName()}' has no primary key.");
                }
                else
                {
                    select = PostgreSqlSqlBuilder.SelectByKey(map, staging, keys, generated, nullSafeKeys);
                }

                var rows = await scope.ReadOutputRowsAsync(select, generated.Count, isAsync, cancellationToken).ConfigureAwait(false);
                OutputWriteBack.Apply(entities, operation.Values, generated, rows);
            }

            if (config.CalculateStats)
            {
                config.StatsInfo = stats;
            }

            await unit.CommitAsync(cancellationToken).ConfigureAwait(false);
            operation.Progress?.Invoke(1m);
        }
        finally
        {
            if (!unit.OwnsTransaction)
            {
                // Inside the caller's transaction the table would otherwise live until their commit.
                await scope.ExecuteQuietlyAsync(PostgreSqlSqlBuilder.DropStagingTable(staging), isAsync).ConfigureAwait(false);
            }
        }
    }

    public static async Task ReadAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var staging = PostgreSqlSqlBuilder.StagingTableName(map, Guid.NewGuid().ToString("N")[..8]);
        var readColumns = map.ReadColumns;
        var nullSafeKeys = NullSafeKeys(map.MatchColumns, operation.Entities, operation.Values);

        await using var unit = await OperationUnit.BeginAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        var scope = unit.Scope;
        try
        {
            await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.CreateStagingTable(staging, map.WriteColumns, onCommitDrop: unit.OwnsTransaction), isAsync, cancellationToken).ConfigureAwait(false);
            await PostgreSqlBulkAdapter.CopyAsync(operation, scope, staging, map.WriteColumns, includeIndex: true, isAsync, cancellationToken).ConfigureAwait(false);

            var rows = await scope.ReadOutputRowsAsync(PostgreSqlSqlBuilder.SelectByKey(map, staging, map.MatchColumns, readColumns, nullSafeKeys), readColumns.Count, isAsync, cancellationToken).ConfigureAwait(false);
            OutputWriteBack.Apply(operation.Entities, operation.Values, readColumns, rows);

            await unit.CommitAsync(cancellationToken).ConfigureAwait(false);
            operation.Progress?.Invoke(1m);
        }
        finally
        {
            if (!unit.OwnsTransaction)
            {
                await scope.ExecuteQuietlyAsync(PostgreSqlSqlBuilder.DropStagingTable(staging), isAsync).ConfigureAwait(false);
            }
        }
    }

    public static async Task TruncateAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var scope = await ConnectionScope.OpenAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.Truncate(map), isAsync, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await scope.DisposeAsync(isAsync).ConfigureAwait(false);
        }
    }

    /// <summary>The nullable match columns for which at least one staged row holds NULL; only those need a null-safe comparison.</summary>
    private static IReadOnlySet<ColumnMap> NullSafeKeys<T>(IReadOnlyList<ColumnMap> keys, IList<T> entities, BulkValueContext values) where T : class
    {
        var result = new HashSet<ColumnMap>();
        foreach (var key in keys)
        {
            if (!key.IsNullable)
            {
                continue;
            }

            foreach (var entity in entities)
            {
                if (entity is not null && key.GetProviderValue(entity, values) is null)
                {
                    result.Add(key);
                    break;
                }
            }
        }

        return result;
    }
}
