using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Output;
using TSD.BulkExtensions.Streaming;
using TSD.BulkExtensions.Transactions;

namespace TSD.BulkExtensions.SqlServer;

/// <summary>
/// The staged path: bulk-copy the entities into a session temp table, run one MERGE (or SELECT for reads) against it,
/// read generated values back through an OUTPUT table, drop the temp tables. Everything runs on the context's
/// connection and joins the caller's transaction; without one, the executor opens its own so the statement and the
/// write-back succeed or fail together.
/// </summary>
internal static class SqlServerMergeExecutor
{
    public static async Task ExecuteAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var config = operation.Config;
        var entities = operation.Entities;
        var operationType = operation.OperationType;

        if (operationType == OperationType.Update && map.UpdateColumns.Count == 0)
        {
            if (config.IgnoreUnknownPropertyNames)
            {
                return; // a graph pass whose type has none of the listed properties: nothing to do for it
            }

            throw new InvalidBulkConfigException("BulkUpdate has no columns to update after applying the include/exclude options.");
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

        await using var unit = await StagedUnit.BeginAsync(operation, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await ExecuteNonQueryAsync(unit.Scope, createTables, isAsync, cancellationToken).ConfigureAwait(false);
            await CopyToStagingAsync(operation, unit.Scope, staging, stagingColumns, isAsync, cancellationToken).ConfigureAwait(false);

            if (identityInsert)
            {
                await ExecuteNonQueryAsync(unit.Scope, SqlServerSqlBuilder.IdentityInsert(map, on: true), isAsync, cancellationToken).ConfigureAwait(false);
            }

            try
            {
                await ExecuteNonQueryAsync(unit.Scope, merge, isAsync, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                if (identityInsert)
                {
                    await ExecuteQuietlyAsync(unit.Scope, SqlServerSqlBuilder.IdentityInsert(map, on: false), isAsync).ConfigureAwait(false);
                }
            }

            if (output is not null)
            {
                var rows = await ReadRowsAsync(unit.Scope, SqlServerSqlBuilder.SelectOutput(output, outputColumns), outputColumns.Count, isAsync, cancellationToken).ConfigureAwait(false);
                var stats = OutputWriteBack.Apply(entities, operation.Values, outputColumns, rows);
                if (config.CalculateStats)
                {
                    config.StatsInfo = stats;
                }
            }

            await unit.CommitAsync(isAsync, cancellationToken).ConfigureAwait(false);
            operation.Progress?.Invoke(1m);
        }
        finally
        {
            await ExecuteQuietlyAsync(unit.Scope, SqlServerSqlBuilder.DropTempTables(output is null ? new[] { staging } : new[] { staging, output }), isAsync).ConfigureAwait(false);
        }
    }

    public static async Task ReadAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var suffix = Guid.NewGuid().ToString("N")[..8];
        var staging = SqlServerSqlBuilder.StagingTableName(map, suffix);
        var readColumns = map.ReadColumns;

        await using var unit = await StagedUnit.BeginAsync(operation, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await ExecuteNonQueryAsync(unit.Scope, SqlServerSqlBuilder.CreateStagingTable(staging, map.WriteColumns), isAsync, cancellationToken).ConfigureAwait(false);
            await CopyToStagingAsync(operation, unit.Scope, staging, map.WriteColumns, isAsync, cancellationToken).ConfigureAwait(false);

            var rows = await ReadRowsAsync(unit.Scope, SqlServerSqlBuilder.SelectRead(map, staging, readColumns), readColumns.Count, isAsync, cancellationToken).ConfigureAwait(false);
            OutputWriteBack.Apply(operation.Entities, operation.Values, readColumns, rows);

            await unit.CommitAsync(isAsync, cancellationToken).ConfigureAwait(false);
            operation.Progress?.Invoke(1m);
        }
        finally
        {
            await ExecuteQuietlyAsync(unit.Scope, SqlServerSqlBuilder.DropTempTables(new[] { staging }), isAsync).ConfigureAwait(false);
        }
    }

    public static async Task TruncateAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var scope = isAsync ? await ConnectionScope.OpenAsync(operation.Context, cancellationToken).ConfigureAwait(false) : ConnectionScope.Open(operation.Context);
        try
        {
            await ExecuteNonQueryAsync(scope, SqlServerSqlBuilder.Truncate(map), isAsync, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            await scope.DisposeAsync(isAsync).ConfigureAwait(false);
        }
    }

    // ---- helpers -----------------------------------------------------------------------------------------------------

    /// <summary>The open connection plus a transaction of our own when the caller has none, so MERGE and write-back are atomic.</summary>
    private sealed class StagedUnit : IAsyncDisposable
    {
        private readonly bool _isAsync;
        private IDbContextTransaction? _transaction;

        private StagedUnit(ConnectionScope scope, IDbContextTransaction? transaction, bool isAsync)
        {
            Scope = scope;
            _transaction = transaction;
            _isAsync = isAsync;
        }

        public ConnectionScope Scope { get; }

        public static async Task<StagedUnit> BeginAsync<T>(BulkOperation<T> operation, bool isAsync, CancellationToken cancellationToken) where T : class
        {
            var context = operation.Context;
            IDbContextTransaction? transaction = null;
            if (context.Database.CurrentTransaction is null && System.Transactions.Transaction.Current is null)
            {
                transaction = isAsync
                    ? await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false)
                    : context.Database.BeginTransaction();
            }

            var scope = isAsync ? await ConnectionScope.OpenAsync(context, cancellationToken).ConfigureAwait(false) : ConnectionScope.Open(context);
            return new StagedUnit(scope, transaction, isAsync);
        }

        public async Task CommitAsync(bool isAsync, CancellationToken cancellationToken)
        {
            if (_transaction is null)
            {
                return;
            }

            if (isAsync)
            {
                await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                _transaction.Commit();
            }
        }

        /// <summary>Releases the connection, then the transaction (rolling it back if it was never committed).</summary>
        public async ValueTask DisposeAsync()
        {
            await Scope.DisposeAsync(_isAsync).ConfigureAwait(false);

            if (_transaction is not null)
            {
                var transaction = _transaction;
                _transaction = null;
                if (_isAsync)
                {
                    await transaction.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    transaction.Dispose();
                }
            }
        }
    }

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

    /// <summary>Reads rows shaped as (index, action, generated columns...) into <see cref="OutputRow"/>s.</summary>
    private static async Task<List<OutputRow>> ReadRowsAsync(ConnectionScope scope, string sql, int valueCount, bool isAsync, CancellationToken cancellationToken)
    {
        var rows = new List<OutputRow>();
        using var command = scope.CreateCommand(sql);
        using var reader = isAsync ? await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false) : command.ExecuteReader();

        while (isAsync ? await reader.ReadAsync(cancellationToken).ConfigureAwait(false) : reader.Read())
        {
            int? index = reader.IsDBNull(0) ? null : reader.GetInt32(0);
            var actionCode = reader.GetString(1)[0];
            var action = actionCode == 'R' ? OutputAction.Read : OutputWriteBack.FromMergeAction(actionCode);
            var values = new object?[valueCount];
            for (var i = 0; i < valueCount; i++)
            {
                values[i] = reader.IsDBNull(i + 2) ? null : reader.GetValue(i + 2);
            }

            rows.Add(new OutputRow(index, action, values));
        }

        return rows;
    }

    private static async Task ExecuteNonQueryAsync(ConnectionScope scope, string sql, bool isAsync, CancellationToken cancellationToken)
    {
        using var command = scope.CreateCommand(sql);
        if (isAsync)
        {
            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            command.ExecuteNonQuery();
        }
    }

    /// <summary>Cleanup statements run in finally blocks: their own failure must never replace the exception being propagated.</summary>
    private static async Task ExecuteQuietlyAsync(ConnectionScope scope, string sql, bool isAsync)
    {
        try
        {
            await ExecuteNonQueryAsync(scope, sql, isAsync, CancellationToken.None).ConfigureAwait(false);
        }
        catch (DbException)
        {
            // Temp tables and IDENTITY_INSERT die with the session anyway.
        }
        catch (InvalidOperationException)
        {
            // Connection already broken by the failing statement.
        }
    }
}
