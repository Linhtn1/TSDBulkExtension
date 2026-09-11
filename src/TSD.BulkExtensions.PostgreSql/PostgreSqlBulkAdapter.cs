namespace TSD.BulkExtensions.PostgreSql;

/// <summary>
/// PostgreSQL implementation: COPY BINARY for streaming, INSERT ... ON CONFLICT / UPDATE ... FROM / DELETE ... USING for set operations.
/// Operations are implemented in phase P2; the adapter is registered and discoverable from P0.
/// </summary>
public sealed class PostgreSqlBulkAdapter : IBulkAdapter
{
    /// <inheritdoc />
    public string ProviderNameSuffix => "PostgreSQL";

    /// <inheritdoc />
    public void Execute<T>(BulkOperation<T> operation) where T : class
    {
        ArgumentNullException.ThrowIfNull(operation);
        throw new NotImplementedException($"PostgreSQL {operation.OperationType} is implemented in phase P2.");
    }

    /// <inheritdoc />
    public Task ExecuteAsync<T>(BulkOperation<T> operation, CancellationToken cancellationToken) where T : class
    {
        ArgumentNullException.ThrowIfNull(operation);
        throw new NotImplementedException($"PostgreSQL {operation.OperationType} is implemented in phase P2.");
    }
}
