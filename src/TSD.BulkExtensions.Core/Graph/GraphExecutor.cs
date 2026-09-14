using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace TSD.BulkExtensions.Graph;

/// <summary>
/// Saves an object graph with the provider adapter, one entity type per pass, parents before children.
/// After each pass the generated keys are copied into the dependents' foreign keys, so the next pass can insert
/// them. Runs inside the ambient transaction, or opens one so a failure leaves nothing half-written.
/// </summary>
internal static class GraphExecutor
{
    public static void Execute<T>(BulkOperation<T> operation, IBulkAdapter adapter) where T : class
        => SyncBridge.Run(ExecuteCoreAsync(operation, adapter, isAsync: false, CancellationToken.None));

    public static Task ExecuteAsync<T>(BulkOperation<T> operation, IBulkAdapter adapter, CancellationToken cancellationToken) where T : class
        => ExecuteCoreAsync(operation, adapter, isAsync: true, cancellationToken);

    private static async Task ExecuteCoreAsync<T>(BulkOperation<T> operation, IBulkAdapter adapter, bool isAsync, CancellationToken cancellationToken) where T : class
    {
        var context = operation.Context;
        var nodes = EntityGraphWalker.Walk(context, operation.Entities);
        if (nodes.Count == 0)
        {
            return;
        }

        var shadowValues = new Dictionary<(object Entity, string Property), object?>(EntityPropertyComparer.Instance);
        var childConfig = CreateChildConfig(operation.Config, context, shadowValues);
        var stats = operation.Config.CalculateStats ? new StatsInfo() : null;

        var ownsTransaction = context.Database.CurrentTransaction is null && System.Transactions.Transaction.Current is null;
        IDbContextTransaction? transaction = null;
        if (ownsTransaction)
        {
            transaction = isAsync
                ? await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false)
                : context.Database.BeginTransaction();
        }

        try
        {
            var remaining = nodes;
            while (remaining.Count > 0)
            {
                var ready = remaining.Where(n => n.Principals.All(p => p.Principal.Saved)).ToList();
                if (ready.Count == 0)
                {
                    throw new BulkExtensionsException(
                        "IncludeGraph cannot order the entities: the graph contains a cycle of foreign keys among " +
                        string.Join(", ", remaining.Select(n => n.EntityType.DisplayName()).Distinct()) + ".");
                }

                foreach (var group in ready.GroupBy(n => n.EntityType.ClrType))
                {
                    var entities = DistinctByKey(group, context);
                    var child = new BulkOperation<object>(context, group.Key, entities, operation.OperationType, childConfig, progress: null);

                    if (isAsync)
                    {
                        await adapter.ExecuteAsync(child, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        adapter.Execute(child);
                    }

                    if (stats is not null && childConfig.StatsInfo is { } passStats)
                    {
                        stats.StatsNumberInserted += passStats.StatsNumberInserted;
                        stats.StatsNumberUpdated += passStats.StatsNumberUpdated;
                        stats.StatsNumberDeleted += passStats.StatsNumberDeleted;
                        childConfig.StatsInfo = null;
                    }

                    foreach (var node in group)
                    {
                        node.Saved = true;
                        foreach (var (dependent, foreignKey) in node.Dependents)
                        {
                            PropagateKey(context, node.Entity, dependent.Entity, foreignKey, shadowValues);
                        }
                    }
                }

                remaining = remaining.Where(n => !n.Saved).ToList();
            }

            if (transaction is not null)
            {
                if (isAsync)
                {
                    await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    transaction.Commit();
                }
            }

            if (stats is not null)
            {
                operation.Config.StatsInfo = stats;
            }

            operation.Progress?.Invoke(1m);
        }
        finally
        {
            if (transaction is not null)
            {
                // Disposing an uncommitted transaction rolls it back.
                if (isAsync)
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

    /// <summary>Per-type passes need generated keys, positional mapping and shadow FKs; the graph flag itself must not recurse.</summary>
    private static BulkConfig CreateChildConfig(BulkConfig config, DbContext context, Dictionary<(object, string), object?> shadowValues)
    {
        var child = config.Clone();
        child.IncludeGraph = false;
        child.SetOutputIdentity = true;
        child.PreserveInsertOrder = true;
        child.EnableShadowProperties = true;
        child.IgnoreUnknownPropertyNames = true;
        child.StatsInfo = null;

        var userShadow = config.ShadowPropertyValue;
        child.ShadowPropertyValue = (entity, property) =>
            shadowValues.TryGetValue((entity, property), out var value) ? value
            : userShadow is not null ? userShadow(entity, property)
            : context.Entry(entity).Property(property).CurrentValue;

        return child;
    }

    /// <summary>Copies the principal's key into the dependent's foreign key, in memory (CLR member) or in the shadow store.</summary>
    private static void PropagateKey(DbContext context, object principal, object dependent, IForeignKey foreignKey, Dictionary<(object, string), object?> shadowValues)
    {
        for (var i = 0; i < foreignKey.Properties.Count; i++)
        {
            var value = ReadProperty(context, principal, foreignKey.PrincipalKey.Properties[i]);
            var target = foreignKey.Properties[i];

            if (target.IsShadowProperty())
            {
                shadowValues[(dependent, target.Name)] = value;
            }
            else if (target.PropertyInfo is { } propertyInfo)
            {
                propertyInfo.SetValue(dependent, value);
            }
            else if (target.FieldInfo is { } fieldInfo)
            {
                fieldInfo.SetValue(dependent, value);
            }
            else
            {
                throw new BulkExtensionsException($"Foreign key '{target.DeclaringType.DisplayName()}.{target.Name}' has neither a property nor a field to assign.");
            }
        }
    }

    private static object? ReadProperty(DbContext context, object entity, IProperty property)
        => property.IsShadowProperty()
            ? context.Entry(entity).Property(property.Name).CurrentValue
            : property.GetGetter().GetClrValue(entity);

    /// <summary>
    /// The same row can appear as several instances (deserialised trees). Only the first instance with a given key is
    /// written; the others already carry the key their dependents need.
    /// </summary>
    private static List<object> DistinctByKey(IGrouping<Type, GraphNode> group, DbContext context)
    {
        var result = new List<object>();
        var key = group.First().EntityType.FindPrimaryKey();
        if (key is null)
        {
            result.AddRange(group.Select(n => n.Entity));
            return result;
        }

        var defaults = key.Properties.Select(p => GetDefault(p.ClrType)).ToArray();
        HashSet<KeyValues>? seen = null;

        foreach (var node in group)
        {
            var values = new object?[key.Properties.Count];
            var allSet = true;
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = ReadProperty(context, node.Entity, key.Properties[i]);
                if (values[i] is null || values[i]!.Equals(defaults[i]))
                {
                    allSet = false;
                }
            }

            if (!allSet || (seen ??= new HashSet<KeyValues>()).Add(new KeyValues(values)))
            {
                result.Add(node.Entity);
            }
        }

        return result;
    }

    private static object? GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(Nullable.GetUnderlyingType(type) ?? type) : null;

    private sealed class KeyValues : IEquatable<KeyValues>
    {
        private readonly object?[] _values;

        public KeyValues(object?[] values) => _values = values;

        public bool Equals(KeyValues? other) => other is not null && _values.SequenceEqual(other._values);

        public override bool Equals(object? obj) => Equals(obj as KeyValues);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var value in _values)
            {
                hash.Add(value);
            }

            return hash.ToHashCode();
        }
    }

    private sealed class EntityPropertyComparer : IEqualityComparer<(object Entity, string Property)>
    {
        public static readonly EntityPropertyComparer Instance = new();

        public bool Equals((object Entity, string Property) x, (object Entity, string Property) y)
            => ReferenceEquals(x.Entity, y.Entity) && string.Equals(x.Property, y.Property, StringComparison.Ordinal);

        public int GetHashCode((object Entity, string Property) obj)
            => HashCode.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj.Entity), obj.Property);
    }
}
