using Microsoft.Data.SqlClient;

namespace TSD.BulkExtensions;

/// <summary>
/// Options for a single bulk operation. Property names and types match EFCore.BulkExtensions so existing call sites compile unchanged.
/// Options that TSD.BulkExtensions does not support are deliberately absent: unsupported usage fails at compile time.
/// The library never mutates the instance it is given; implied options are applied to a per-operation copy.
/// </summary>
public class BulkConfig
{
    /// <summary>
    /// Keep the order of the list when inserting so that generated identity values can be mapped back by position.
    /// Default <c>true</c>. Forced to <c>true</c> when <see cref="IncludeGraph"/> is set.
    /// </summary>
    public bool PreserveInsertOrder { get; set; } = true;

    /// <summary>
    /// After the operation, write database-generated values (identity, sequential GUID default, rowversion) back into the entities.
    /// Turns <see cref="OperationType.Insert"/> from the direct-copy fast path into the temp-table + merge path.
    /// </summary>
    public bool SetOutputIdentity { get; set; }

    /// <summary>Rows per batch handed to the streaming API (SqlBulkCopy.BatchSize). Default 2000; 0 means a single batch.</summary>
    public int BatchSize { get; set; } = 2000;

    /// <summary>Invoke the progress callback after this many rows. Defaults to <see cref="BatchSize"/>.</summary>
    public int? NotifyAfter { get; set; }

    /// <summary>Timeout in seconds for the streaming phase. <c>null</c> uses the provider default.</summary>
    public int? BulkCopyTimeout { get; set; }

    /// <summary>Stream rows to the server instead of buffering them (SqlBulkCopy.EnableStreaming).</summary>
    public bool EnableStreaming { get; set; }

    /// <summary>
    /// Create the staging table as a session temp table (<c>#Name</c> on SQL Server, <c>TEMP TABLE</c> on PostgreSQL) instead of a
    /// regular table in the target schema. Requires an open transaction on the context for every operation except a plain insert.
    /// </summary>
    public bool UseTempDB { get; set; }

    /// <summary>Append a random suffix to the staging table name so concurrent operations on the same table do not collide. Default <c>true</c>.</summary>
    public bool UniqueTableNameTempDb { get; set; } = true;

    /// <summary>Write to this table instead of the one the entity is mapped to. Accepts <c>schema.table</c>.</summary>
    public string? CustomDestinationTableName { get; set; }

    /// <summary>Only these properties are written (and, unless overridden, compared and updated). Key properties are always included.</summary>
    public List<string>? PropertiesToInclude { get; set; }

    /// <summary>Every mapped property except these is written. Cannot be combined with <see cref="PropertiesToInclude"/>.</summary>
    public List<string>? PropertiesToExclude { get; set; }

    /// <summary>Only these properties decide whether a matched row counts as changed.</summary>
    public List<string>? PropertiesToIncludeOnCompare { get; set; }

    /// <summary>These properties are ignored when deciding whether a matched row counts as changed.</summary>
    public List<string>? PropertiesToExcludeOnCompare { get; set; }

    /// <summary>Only these properties are written in the UPDATE branch of a merge.</summary>
    public List<string>? PropertiesToIncludeOnUpdate { get; set; }

    /// <summary>These properties are never written in the UPDATE branch of a merge.</summary>
    public List<string>? PropertiesToExcludeOnUpdate { get; set; }

    /// <summary>Match rows on these properties instead of the primary key. PostgreSQL requires a unique index covering them.</summary>
    public List<string>? UpdateByProperties { get; set; }

    /// <summary>
    /// Extra SQL predicate appended to the update branch. Receives the target and source aliases
    /// (<c>T</c>/<c>S</c> on SQL Server, table name/<c>EXCLUDED</c> on PostgreSQL).
    /// </summary>
    public Func<string, string, string>? OnConflictUpdateWhereSql { get; set; }

    /// <summary>Include EF shadow properties (for example FK columns without a CLR property) in the operation.</summary>
    public bool EnableShadowProperties { get; set; }

    /// <summary>Supplies the value of a shadow property for an entity. Receives the entity and the property name.</summary>
    public Func<object, string, object?>? ShadowPropertyValue { get; set; }

    /// <summary>Treat rowversion/timestamp columns like ordinary columns instead of excluding them from writes.</summary>
    public bool IgnoreRowVersion { get; set; }

    /// <summary>Emit <c>WITH (HOLDLOCK)</c> on the MERGE statement (SQL Server only). Default <c>true</c>.</summary>
    public bool WithHoldlock { get; set; } = true;

    /// <summary>Flags passed to SqlBulkCopy (SQL Server only). Ignored by other providers.</summary>
    public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; } = SqlBulkCopyOptions.Default;

    /// <summary>Also insert/update the navigation graph reachable from each entity, parents before children, in one transaction.</summary>
    public bool IncludeGraph { get; set; }

    /// <summary>Populate <see cref="StatsInfo"/> with inserted/updated/deleted counts after a merge-style operation.</summary>
    public bool CalculateStats { get; set; }

    /// <summary>Row counts, set on the caller's instance when <see cref="CalculateStats"/> is enabled.</summary>
    public StatsInfo? StatsInfo { get; set; }

    /// <summary>Shallow copy used as the effective configuration of one operation, so the caller's instance stays untouched.</summary>
    internal BulkConfig Clone() => (BulkConfig)MemberwiseClone();

    /// <summary>
    /// Validates option combinations that cannot work together and applies implied options.
    /// Runs on the per-operation copy produced by <see cref="Clone"/>.
    /// </summary>
    internal void Prepare(OperationType operationType)
    {
        if (PropertiesToInclude is { Count: > 0 } && PropertiesToExclude is { Count: > 0 })
        {
            throw new InvalidBulkConfigException($"{nameof(PropertiesToInclude)} and {nameof(PropertiesToExclude)} cannot both be set.");
        }

        if (PropertiesToIncludeOnCompare is { Count: > 0 } && PropertiesToExcludeOnCompare is { Count: > 0 })
        {
            throw new InvalidBulkConfigException($"{nameof(PropertiesToIncludeOnCompare)} and {nameof(PropertiesToExcludeOnCompare)} cannot both be set.");
        }

        if (PropertiesToIncludeOnUpdate is { Count: > 0 } && PropertiesToExcludeOnUpdate is { Count: > 0 })
        {
            throw new InvalidBulkConfigException($"{nameof(PropertiesToIncludeOnUpdate)} and {nameof(PropertiesToExcludeOnUpdate)} cannot both be set.");
        }

        // 0 means "one batch" for SqlBulkCopy, so only negatives are invalid.
        if (BatchSize < 0)
        {
            throw new InvalidBulkConfigException($"{nameof(BatchSize)} cannot be negative.");
        }

        if (IncludeGraph)
        {
            if (operationType is not (OperationType.Insert or OperationType.InsertOrUpdate or OperationType.InsertOrUpdateOrDelete or OperationType.Update))
            {
                throw new InvalidBulkConfigException($"{nameof(IncludeGraph)} only supports Insert, InsertOrUpdate, InsertOrUpdateOrDelete and Update.");
            }

            // Children need the generated keys of their parents, and the mapping is positional.
            SetOutputIdentity = true;
            PreserveInsertOrder = true;
            EnableShadowProperties = true;
        }
    }
}
