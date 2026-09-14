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
    /// Kept for compatibility with EFCore.BulkExtensions. Staging always uses a session-local temp table
    /// (<c>#Name</c> on SQL Server, <c>TEMP TABLE</c> on PostgreSQL) held on the context's open connection, so the option has no effect.
    /// </summary>
    public bool UseTempDB { get; set; }

    /// <summary>
    /// Kept for compatibility with EFCore.BulkExtensions. The staging table name always gets a per-operation suffix, so the option has no effect.
    /// </summary>
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

    /// <summary>Match rows on these properties instead of the primary key. No unique index is required on either provider.</summary>
    public List<string>? UpdateByProperties { get; set; }

    /// <summary>
    /// Extra SQL predicate appended to the update branch. Receives the target and source (staging) aliases:
    /// <c>T</c>/<c>S</c> on SQL Server, <c>t</c>/<c>s</c> on PostgreSQL. Quote column names for the provider.
    /// </summary>
    public Func<string, string, string>? OnConflictUpdateWhereSql { get; set; }

    /// <summary>Include EF shadow properties (for example FK columns without a CLR property) in the operation.</summary>
    public bool EnableShadowProperties { get; set; }

    /// <summary>Supplies the value of a shadow property for an entity. Receives the entity and the property name.</summary>
    public Func<object, string, object?>? ShadowPropertyValue { get; set; }

    /// <summary>
    /// Treat concurrency-token columns like ordinary columns instead of excluding them from writes. Only meaningful when the
    /// property is flagged as a concurrency token but the column is not a real server-maintained rowversion/timestamp
    /// (SQL Server refuses explicit values for those).
    /// </summary>
    public bool IgnoreRowVersion { get; set; }

    /// <summary>Emit <c>WITH (HOLDLOCK)</c> on the MERGE statement (SQL Server only). Default <c>true</c>.</summary>
    public bool WithHoldlock { get; set; } = true;

    /// <summary>
    /// Flags passed to SqlBulkCopy. <see cref="Microsoft.Data.SqlClient.SqlBulkCopyOptions.KeepIdentity"/> is honoured by
    /// every provider (explicit identity values are inserted; PostgreSQL then moves the sequence past them); the other
    /// flags only affect SQL Server.
    /// </summary>
    public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; } = SqlBulkCopyOptions.Default;

    /// <summary>Whether explicit identity values are written (the KeepIdentity flag), on any provider.</summary>
    internal bool KeepIdentity => SqlBulkCopyOptions.HasFlag(SqlBulkCopyOptions.KeepIdentity);

    /// <summary>Rows between two progress callbacks: <see cref="NotifyAfter"/>, else <see cref="BatchSize"/>, else the whole list (single batch).</summary>
    internal int GetNotifyAfter(int totalRows)
    {
        var notifyAfter = NotifyAfter ?? BatchSize;
        return notifyAfter > 0 ? notifyAfter : Math.Max(1, totalRows);
    }

    /// <summary>
    /// Also insert/update the navigation graph reachable from each entity, parents before children, in one transaction.
    /// Supported with Insert, InsertOrUpdate and Update. Implies <see cref="SetOutputIdentity"/>, <see cref="PreserveInsertOrder"/>
    /// and <see cref="EnableShadowProperties"/>. The include/exclude lists apply to every entity type in the graph; names a
    /// type does not have are skipped for that type.
    /// </summary>
    public bool IncludeGraph { get; set; }

    /// <summary>Populate <see cref="StatsInfo"/> with inserted/updated/deleted counts after a merge-style operation.</summary>
    public bool CalculateStats { get; set; }

    /// <summary>Row counts, set on the caller's instance when <see cref="CalculateStats"/> is enabled.</summary>
    public StatsInfo? StatsInfo { get; set; }

    /// <summary>
    /// Skip, instead of rejecting, include/exclude names that a particular entity type does not have. Set by the graph
    /// executor, which applies one option set to every type in the graph.
    /// </summary>
    internal bool IgnoreUnknownPropertyNames { get; set; }

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
            // Sync is excluded on purpose: a per-type pass would delete every row of that table not in the pass.
            if (operationType is not (OperationType.Insert or OperationType.InsertOrUpdate or OperationType.Update))
            {
                throw new InvalidBulkConfigException($"{nameof(IncludeGraph)} only supports Insert, InsertOrUpdate and Update.");
            }

            // Children need the generated keys of their parents, and the mapping is positional.
            SetOutputIdentity = true;
            PreserveInsertOrder = true;
            EnableShadowProperties = true;
        }
    }
}
