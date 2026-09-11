namespace TSD.BulkExtensions.SqlServer;

/// <summary>
/// SQL Server implementation: SqlBulkCopy for streaming, MERGE with OUTPUT for set operations.
/// Operations are implemented in phase P1; the adapter is registered and discoverable from P0.
/// </summary>
public sealed class SqlServerBulkAdapter : IBulkAdapter
{
    /// <inheritdoc />
    public string ProviderNameSuffix => "SqlServer";

    /// <inheritdoc />
    public void Execute<T>(BulkOperation<T> operation) where T : class
    {
        ArgumentNullException.ThrowIfNull(operation);
        throw new NotImplementedException($"SQL Server {operation.OperationType} is implemented in phase P1.");
    }

    /// <inheritdoc />
    public Task ExecuteAsync<T>(BulkOperation<T> operation, CancellationToken cancellationToken) where T : class
    {
        ArgumentNullException.ThrowIfNull(operation);
        throw new NotImplementedException($"SQL Server {operation.OperationType} is implemented in phase P1.");
    }
}
