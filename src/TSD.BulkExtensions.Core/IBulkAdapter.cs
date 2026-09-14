using Microsoft.EntityFrameworkCore.Metadata;

namespace TSD.BulkExtensions;

/// <summary>
/// Provider-specific execution of bulk operations. One implementation per database engine.
/// Adapters are stateless and may be shared across threads.
/// </summary>
public interface IBulkAdapter
{
    /// <summary>
    /// Suffix of <c>DbContext.Database.ProviderName</c> this adapter serves, for example <c>SqlServer</c> or <c>PostgreSQL</c>.
    /// Compared case-insensitively.
    /// </summary>
    string ProviderNameSuffix { get; }

    /// <summary>
    /// Whether the column backing <paramref name="property"/> is a server-assigned identity/serial column.
    /// Identity detection is provider-specific (SQL Server value generation strategy, PostgreSQL identity/serial).
    /// </summary>
    bool IsIdentityColumn(IProperty property);

    /// <summary>Executes the operation synchronously.</summary>
    void Execute<T>(BulkOperation<T> operation) where T : class;

    /// <summary>Executes the operation asynchronously.</summary>
    Task ExecuteAsync<T>(BulkOperation<T> operation, CancellationToken cancellationToken) where T : class;
}
