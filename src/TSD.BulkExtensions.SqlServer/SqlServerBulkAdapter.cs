using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Streaming;
using TSD.BulkExtensions.Transactions;

namespace TSD.BulkExtensions.SqlServer;

/// <summary>
/// SQL Server implementation. A plain insert streams straight into the table with SqlBulkCopy; every other operation
/// stages the rows in a session temp table and runs one MERGE (see <see cref="SqlServerMergeExecutor"/>).
/// </summary>
public sealed class SqlServerBulkAdapter : IBulkAdapter
{
    /// <inheritdoc />
    public string ProviderNameSuffix => "SqlServer";

    /// <inheritdoc />
    public bool IsIdentityColumn(IProperty property)
    {
        ArgumentNullException.ThrowIfNull(property);
        return property.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn;
    }

    /// <inheritdoc />
    public void Execute<T>(BulkOperation<T> operation) where T : class
        => SyncBridge.Run(ExecuteCoreAsync(operation, isAsync: false, CancellationToken.None));

    /// <inheritdoc />
    public Task ExecuteAsync<T>(BulkOperation<T> operation, CancellationToken cancellationToken) where T : class
        => ExecuteCoreAsync(operation, isAsync: true, cancellationToken);

    private async Task ExecuteCoreAsync<T>(BulkOperation<T> operation, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        ArgumentNullException.ThrowIfNull(operation);

        var map = EntityTableMap.Create(operation.Context, operation.EntityType, operation.Config, operation.OperationType, IsIdentityColumn);

        switch (operation.OperationType)
        {
            case OperationType.Insert when !operation.Config.SetOutputIdentity:
                await InsertDirectAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
            case OperationType.Truncate:
                await SqlServerMergeExecutor.TruncateAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
            case OperationType.Read:
                await SqlServerMergeExecutor.ReadAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
            default:
                await SqlServerMergeExecutor.ExecuteAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
        }
    }

    // ---- Fast path: SqlBulkCopy straight into the destination table --------------------------------------------------

    private static async Task InsertDirectAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var context = operation.Context;
        var (insertColumns, _) = map.GetInsertColumns(operation.Entities, operation.Values);

        var scope = await ConnectionScope.OpenAsync(context, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            using var bulkCopy = CreateBulkCopy(scope, operation.Config, SqlServerSqlBuilder.TableName(map), insertColumns, operation.Entities.Count, operation.Progress, includeIndexColumn: false);
            using var reader = new EntityDataReader<T>(operation.Entities, operation.Values, insertColumns, includeIndexColumn: false);

            if (isAsync)
            {
                await bulkCopy.WriteToServerAsync(reader, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                bulkCopy.WriteToServer(reader);
            }

            var count = operation.Entities.Count;
            if (operation.Progress is { } progress && count % operation.Config.GetNotifyAfter(count) != 0)
            {
                progress(1m); // SqlRowsCopied did not fire on the last row
            }
        }
        finally
        {
            await scope.DisposeAsync(isAsync).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Configures SqlBulkCopy from <see cref="BulkConfig"/>: batch size, timeout, streaming, options, progress and column
    /// mappings by name. With <paramref name="includeIndexColumn"/> the row index is mapped too and declared as already
    /// sorted, so the staging table's clustered index costs no sort.
    /// </summary>
    internal static SqlBulkCopy CreateBulkCopy(ConnectionScope scope, BulkConfig config, string destinationTable, IReadOnlyList<ColumnMap> columns, int totalRows, Action<decimal>? progress, bool includeIndexColumn)
    {
        if (scope.Connection is not SqlConnection connection)
        {
            throw new BulkExtensionsException(
                $"SqlBulkCopy needs a SqlConnection but the context uses '{scope.Connection.GetType().FullName}'. Connection wrappers are not supported.");
        }

        var transaction = scope.Transaction as SqlTransaction;
        if (scope.Transaction is not null && transaction is null)
        {
            throw new BulkExtensionsException($"The ambient transaction is a '{scope.Transaction.GetType().FullName}', not a SqlTransaction.");
        }

        var bulkCopy = new SqlBulkCopy(connection, config.SqlBulkCopyOptions, transaction)
        {
            DestinationTableName = destinationTable,
            BatchSize = config.BatchSize,
            EnableStreaming = config.EnableStreaming,
        };

        // Explicit option first, then the context's command timeout, else SqlBulkCopy's own 30 s default.
        var timeout = config.BulkCopyTimeout ?? scope.CommandTimeout;
        if (timeout.HasValue)
        {
            bulkCopy.BulkCopyTimeout = timeout.Value;
        }

        foreach (var column in columns)
        {
            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
        }

        if (includeIndexColumn)
        {
            bulkCopy.ColumnMappings.Add(EntityDataReader<object>.IndexColumnName, EntityDataReader<object>.IndexColumnName);
            bulkCopy.ColumnOrderHints.Add(EntityDataReader<object>.IndexColumnName, SortOrder.Ascending);
        }

        if (progress is not null && totalRows > 0)
        {
            bulkCopy.NotifyAfter = config.GetNotifyAfter(totalRows);
            bulkCopy.SqlRowsCopied += (_, e) => progress(Math.Min(1m, (decimal)e.RowsCopied / totalRows));
        }

        return bulkCopy;
    }
}
