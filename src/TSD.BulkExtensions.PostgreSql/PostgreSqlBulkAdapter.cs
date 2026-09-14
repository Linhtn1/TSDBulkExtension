using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TSD.BulkExtensions.Metadata;
using TSD.BulkExtensions.Transactions;

namespace TSD.BulkExtensions.PostgreSql;

/// <summary>
/// PostgreSQL implementation. A plain insert streams straight into the table with COPY BINARY; every other operation
/// stages the rows in a temp table and runs set statements against it (see <see cref="PostgreSqlMergeExecutor"/>).
/// </summary>
public sealed class PostgreSqlBulkAdapter : IBulkAdapter
{
    /// <inheritdoc />
    public string ProviderNameSuffix => "PostgreSQL";

    /// <inheritdoc />
    public bool IsIdentityColumn(IProperty property)
    {
        ArgumentNullException.ThrowIfNull(property);
        return property.GetValueGenerationStrategy() is NpgsqlValueGenerationStrategy.IdentityAlwaysColumn
            or NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            or NpgsqlValueGenerationStrategy.SerialColumn;
    }

    /// <summary>GENERATED ALWAYS identity columns need <c>OVERRIDING SYSTEM VALUE</c> to accept explicit values.</summary>
    internal static bool IsIdentityAlways(IProperty property)
        => property.GetValueGenerationStrategy() == NpgsqlValueGenerationStrategy.IdentityAlwaysColumn;

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
                await PostgreSqlMergeExecutor.TruncateAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
            case OperationType.Read:
                await PostgreSqlMergeExecutor.ReadAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
            default:
                await PostgreSqlMergeExecutor.ExecuteAsync(operation, map, isAsync, cancellationToken).ConfigureAwait(false);
                break;
        }
    }

    // ---- Fast path: COPY straight into the destination table -------------------------------------------------------

    private static async Task InsertDirectAsync<T>(BulkOperation<T> operation, EntityTableMap map, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var (insertColumns, _) = map.GetInsertColumns(operation.Entities, operation.Values);
        var keepsIdentity = map.KeepIdentity && map.IdentityColumn is not null && insertColumns.Contains(map.IdentityColumn);

        var scope = await ConnectionScope.OpenAsync(operation.Context, isAsync, cancellationToken).ConfigureAwait(false);
        try
        {
            await CopyAsync(operation, scope, PostgreSqlSqlBuilder.TableName(map), insertColumns, includeIndex: false, isAsync, cancellationToken).ConfigureAwait(false);

            if (keepsIdentity)
            {
                await scope.ExecuteNonQueryAsync(PostgreSqlSqlBuilder.SyncSequence(map, map.IdentityColumn!), isAsync, cancellationToken).ConfigureAwait(false);
            }

            var count = operation.Entities.Count;
            if (operation.Progress is { } progress && count % operation.Config.GetNotifyAfter(count) != 0)
            {
                progress(1m); // the row loop did not land on a notification boundary
            }
        }
        finally
        {
            await scope.DisposeAsync(isAsync).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Streams the entities into <paramref name="table"/> with COPY … FROM STDIN (FORMAT BINARY). Progress is reported
    /// every NotifyAfter (or BatchSize) rows; the caller reports completion.
    /// </summary>
    internal static async Task CopyAsync<T>(
        BulkOperation<T> operation,
        ConnectionScope scope,
        string table,
        IReadOnlyList<ColumnMap> columns,
        bool includeIndex,
        bool isAsync,
        CancellationToken cancellationToken) where T : class
    {
        if (scope.Connection is not NpgsqlConnection connection)
        {
            throw new BulkExtensionsException(
                $"COPY needs an NpgsqlConnection but the context uses '{scope.Connection.GetType().FullName}'. Connection wrappers are not supported.");
        }

        var entities = operation.Entities;
        var values = operation.Values;
        var progress = operation.Progress;
        var notifyAfter = progress is null ? 0 : operation.Config.GetNotifyAfter(entities.Count);
        var typeNames = columns.Select(PostgreSqlSqlBuilder.CopyTypeName).ToArray();
        var copy = PostgreSqlSqlBuilder.Copy(table, columns, includeIndex);

        var writer = isAsync
            ? await connection.BeginBinaryImportAsync(copy, cancellationToken).ConfigureAwait(false)
            : connection.BeginBinaryImport(copy);

        try
        {
            if (operation.Config.BulkCopyTimeout is { } timeout)
            {
                writer.Timeout = TimeSpan.FromSeconds(timeout);
            }

            for (var index = 0; index < entities.Count; index++)
            {
                var entity = entities[index] ?? throw new InvalidOperationException($"Entity at index {index} is null.");

                if (isAsync)
                {
                    await writer.StartRowAsync(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    writer.StartRow();
                }

                for (var i = 0; i < columns.Count; i++)
                {
                    await WriteCellAsync(columns[i].GetProviderValue(entity, values), typeNames[i]).ConfigureAwait(false);
                }

                if (includeIndex)
                {
                    await WriteCellAsync(index, "integer").ConfigureAwait(false);
                }

                if (notifyAfter > 0 && (index + 1) % notifyAfter == 0)
                {
                    progress!((decimal)(index + 1) / entities.Count);
                }
            }

            if (isAsync)
            {
                await writer.CompleteAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                writer.Complete();
            }
        }
        finally
        {
            if (isAsync)
            {
                await writer.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                writer.Dispose();
            }
        }

        async ValueTask WriteCellAsync(object? value, string typeName)
        {
            if (value is null)
            {
                if (isAsync)
                {
                    await writer.WriteNullAsync(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    writer.WriteNull();
                }
            }
            else if (isAsync)
            {
                await writer.WriteAsync(value, typeName, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                writer.Write(value, typeName);
            }
        }
    }
}
