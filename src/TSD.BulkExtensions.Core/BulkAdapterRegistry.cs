using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace TSD.BulkExtensions;

/// <summary>
/// Resolves the <see cref="IBulkAdapter"/> for a context's database provider.
/// Provider packages are discovered by assembly name so that referencing <c>TSD.BulkExtensions.SqlServer</c>
/// or <c>TSD.BulkExtensions.PostgreSql</c> is enough; <see cref="Register"/> overrides discovery (for tests or custom adapters).
/// Each provider package embeds an ILLink descriptor that keeps its adapter type when the application is trimmed.
/// </summary>
public static class BulkAdapterRegistry
{
    // provider name suffix -> adapter, either registered explicitly or discovered
    private static readonly ConcurrentDictionary<string, IBulkAdapter> Adapters = new(StringComparer.OrdinalIgnoreCase);

    // full provider name -> adapter, so the hot path is one lock-free lookup
    private static readonly ConcurrentDictionary<string, IBulkAdapter> ByProviderName = new(StringComparer.Ordinal);

    // provider name suffix -> assembly-qualified adapter type name
    private static readonly IReadOnlyDictionary<string, string> KnownAdapters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["SqlServer"] = "TSD.BulkExtensions.SqlServer.SqlServerBulkAdapter, TSD.BulkExtensions.SqlServer",
        ["PostgreSQL"] = "TSD.BulkExtensions.PostgreSql.PostgreSqlBulkAdapter, TSD.BulkExtensions.PostgreSql",
    };

    /// <summary>Registers (or replaces) the adapter used for its <see cref="IBulkAdapter.ProviderNameSuffix"/>.</summary>
    public static void Register(IBulkAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        Adapters[adapter.ProviderNameSuffix] = adapter;
        ByProviderName.Clear();
    }

    /// <summary>Removes every registered adapter. Discovery by assembly name still applies afterwards.</summary>
    public static void Clear()
    {
        Adapters.Clear();
        ByProviderName.Clear();
    }

    /// <summary>Returns the adapter for the context's provider, loading the provider package on first use.</summary>
    /// <exception cref="BulkProviderNotFoundException">No adapter is registered or discoverable for the provider.</exception>
    public static IBulkAdapter Resolve(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var providerName = context.Database.ProviderName
            ?? throw new BulkProviderNotFoundException("DbContext has no database provider configured.");

        if (ByProviderName.TryGetValue(providerName, out var cached))
        {
            return cached;
        }

        var adapter = Find(providerName);
        ByProviderName[providerName] = adapter;
        return adapter;
    }

    private static IBulkAdapter Find(string providerName)
    {
        foreach (var (suffix, adapter) in Adapters)
        {
            if (providerName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                return adapter;
            }
        }

        foreach (var (suffix, typeName) in KnownAdapters)
        {
            if (!providerName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var type = Type.GetType(typeName, throwOnError: false)
                ?? throw new BulkProviderNotFoundException(
                    $"Provider '{providerName}' needs the TSD.BulkExtensions.{suffix} package. Add a reference to it.");

            var adapter = (IBulkAdapter)Activator.CreateInstance(type)!;
            return Adapters.GetOrAdd(suffix, adapter);
        }

        throw new BulkProviderNotFoundException($"No bulk adapter is available for provider '{providerName}'. Supported: {string.Join(", ", KnownAdapters.Keys)}.");
    }
}
