namespace TSD.BulkExtensions;

/// <summary>
/// The kind of bulk operation being executed. Drives which SQL the provider adapter generates.
/// </summary>
public enum OperationType
{
    /// <summary>Insert every entity. Fast path (no temp table) unless <see cref="BulkConfig.SetOutputIdentity"/> is set.</summary>
    Insert,

    /// <summary>Insert entities whose key does not exist, update the ones that do.</summary>
    InsertOrUpdate,

    /// <summary>Like <see cref="InsertOrUpdate"/>, additionally deleting rows in the table that are not in the list.</summary>
    InsertOrUpdateOrDelete,

    /// <summary>Update every entity by key.</summary>
    Update,

    /// <summary>Delete every entity by key.</summary>
    Delete,

    /// <summary>Load the current database values of the entities, matched by key, into the given instances.</summary>
    Read,

    /// <summary>TRUNCATE the entity's table.</summary>
    Truncate,
}
