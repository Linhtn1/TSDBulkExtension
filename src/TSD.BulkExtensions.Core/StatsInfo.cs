namespace TSD.BulkExtensions;

/// <summary>
/// Row counts produced by a merge-style operation when <see cref="BulkConfig.CalculateStats"/> is enabled.
/// </summary>
public sealed class StatsInfo
{
    /// <summary>Number of rows inserted.</summary>
    public int StatsNumberInserted { get; set; }

    /// <summary>Number of rows updated.</summary>
    public int StatsNumberUpdated { get; set; }

    /// <summary>Number of rows deleted.</summary>
    public int StatsNumberDeleted { get; set; }
}
