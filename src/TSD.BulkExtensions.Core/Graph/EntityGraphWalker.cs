using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TSD.BulkExtensions.Graph;

/// <summary>One entity reachable from the roots, with the foreign-key edges that order its save.</summary>
internal sealed class GraphNode
{
    public GraphNode(object entity, IEntityType entityType)
    {
        Entity = entity;
        EntityType = entityType;
    }

    public object Entity { get; }
    public IEntityType EntityType { get; }

    /// <summary>Entities this one carries a foreign key to; they must be saved first.</summary>
    public List<(GraphNode Principal, IForeignKey ForeignKey)> Principals { get; } = new();

    /// <summary>Entities carrying a foreign key to this one; they receive its key once it is saved.</summary>
    public List<(GraphNode Dependent, IForeignKey ForeignKey)> Dependents { get; } = new();

    public bool Saved { get; set; }
}

/// <summary>
/// Flattens an object graph into nodes and foreign-key edges by following EF navigations in both directions.
/// Instances are identified by reference, so an entity reached through several paths is one node.
/// </summary>
internal static class EntityGraphWalker
{
    public static List<GraphNode> Walk(DbContext context, IEnumerable<object?> roots)
    {
        var nodes = new Dictionary<object, GraphNode>(ReferenceEqualityComparer.Instance);
        var order = new List<GraphNode>();
        var edges = new List<(object Dependent, object Principal, IForeignKey ForeignKey)>();
        var pending = new Stack<object>();

        foreach (var root in roots)
        {
            if (root is not null)
            {
                pending.Push(root);
            }
        }

        while (pending.Count > 0)
        {
            var entity = pending.Pop();
            if (nodes.ContainsKey(entity))
            {
                continue;
            }

            // FindRuntimeEntityType also resolves EF proxy types (lazy loading / change tracking) to their mapped type.
            var entityType = context.Model.FindRuntimeEntityType(entity.GetType());
            if (entityType is null)
            {
                continue; // not part of the model: nothing to save
            }

            var node = new GraphNode(entity, entityType);
            nodes[entity] = node;
            order.Add(node);

            foreach (var navigation in entityType.GetNavigations())
            {
                var value = navigation.GetGetter().GetClrValue(entity);
                if (value is null)
                {
                    continue;
                }

                if (navigation.TargetEntityType.IsOwned())
                {
                    if (SharesTable(navigation.TargetEntityType, entityType))
                    {
                        continue; // flattened into the owner's columns by EntityTableMap
                    }

                    throw new NotSupportedException(
                        $"IncludeGraph cannot save '{entityType.DisplayName()}.{navigation.Name}': owned types mapped to their own table are not supported.");
                }

                if (navigation.IsCollection)
                {
                    foreach (var item in (IEnumerable)value)
                    {
                        if (item is null)
                        {
                            continue;
                        }

                        // A collection always sits on the principal side: each item depends on this entity.
                        edges.Add((item, entity, navigation.ForeignKey));
                        pending.Push(item);
                    }
                }
                else
                {
                    if (navigation.IsOnDependent)
                    {
                        edges.Add((entity, value, navigation.ForeignKey));
                    }
                    else
                    {
                        edges.Add((value, entity, navigation.ForeignKey));
                    }

                    pending.Push(value);
                }
            }
        }

        var seen = new HashSet<(GraphNode, GraphNode, IForeignKey)>();
        foreach (var (dependent, principal, foreignKey) in edges)
        {
            if (!nodes.TryGetValue(dependent, out var dependentNode) || !nodes.TryGetValue(principal, out var principalNode))
            {
                continue;
            }

            if (ReferenceEquals(dependentNode, principalNode) || !seen.Add((dependentNode, principalNode, foreignKey)))
            {
                continue;
            }

            dependentNode.Principals.Add((principalNode, foreignKey));
            principalNode.Dependents.Add((dependentNode, foreignKey));
        }

        return order;
    }

    private static bool SharesTable(IEntityType owned, IEntityType owner)
        => owned.GetTableName() == owner.GetTableName() && owned.GetSchema() == owner.GetSchema();
}
