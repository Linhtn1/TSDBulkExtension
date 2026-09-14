using TSD.BulkExtensions.Metadata;

namespace TSD.BulkExtensions.Output;

/// <summary>What the database did with a source row.</summary>
public enum OutputAction
{
    /// <summary>Row was only read (BulkRead); no statistics impact.</summary>
    Read = 0,

    /// <summary>Row was inserted.</summary>
    Insert,

    /// <summary>Row was updated.</summary>
    Update,

    /// <summary>Row was deleted; carries no generated values.</summary>
    Delete,
}

/// <summary>One row the database reported after a set operation: which source row it came from, what happened, and the generated values.</summary>
/// <param name="Index">Position of the source entity in the list, or <c>null</c> for rows that had no source (deleted by a sync).</param>
/// <param name="Action">What the statement did with the row.</param>
/// <param name="Values">Generated values in the order of the output column list.</param>
public readonly record struct OutputRow(int? Index, OutputAction Action, object?[] Values);

/// <summary>
/// Copies server-generated values (identity, defaults, computed, rowversion) back into the entities that produced them,
/// matched by source index, and tallies inserted/updated/deleted counts.
/// </summary>
public static class OutputWriteBack
{
    /// <summary>Maps the first letter of a MERGE <c>$action</c> (I/U/D) to <see cref="OutputAction"/>.</summary>
    public static OutputAction FromMergeAction(char action) => char.ToUpperInvariant(action) switch
    {
        'I' => OutputAction.Insert,
        'U' => OutputAction.Update,
        'D' => OutputAction.Delete,
        _ => throw new BulkExtensionsException($"Unexpected MERGE action '{action}'."),
    };

    /// <summary>
    /// Applies <paramref name="rows"/> to <paramref name="entities"/>. Returns the action counts.
    /// </summary>
    /// <exception cref="BulkExtensionsException">A source row matched more than one table row, so its generated values are ambiguous.</exception>
    public static StatsInfo Apply<T>(IList<T> entities, BulkValueContext values, IReadOnlyList<ColumnMap> columns, IEnumerable<OutputRow> rows)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(columns);
        ArgumentNullException.ThrowIfNull(rows);

        var stats = new StatsInfo();
        var seen = columns.Count > 0 ? new bool[entities.Count] : null;
        List<int>? ambiguous = null;

        foreach (var row in rows)
        {
            switch (row.Action)
            {
                case OutputAction.Insert: stats.StatsNumberInserted++; break;
                case OutputAction.Update: stats.StatsNumberUpdated++; break;
                case OutputAction.Delete: stats.StatsNumberDeleted++; break;
                case OutputAction.Read: break;
                default: throw new BulkExtensionsException($"Unknown output action {row.Action}.");
            }

            if (row.Index is not { } index || row.Action == OutputAction.Delete || seen is null)
            {
                continue;
            }

            if (index < 0 || index >= entities.Count)
            {
                throw new BulkExtensionsException($"Output row refers to source index {index}, outside the {entities.Count} entities written.");
            }

            if (seen[index])
            {
                (ambiguous ??= new List<int>()).Add(index);
                continue;
            }

            seen[index] = true;
            var entity = entities[index]!;
            for (var i = 0; i < columns.Count; i++)
            {
                columns[i].SetFromProviderValue(entity, values, row.Values[i]);
            }
        }

        if (ambiguous is not null)
        {
            throw new BulkExtensionsException(
                "Generated values could not be mapped back: each of the source rows at index " +
                string.Join(", ", ambiguous.Distinct().OrderBy(x => x)) +
                " matched more than one table row. Match on a unique key (UpdateByProperties) when SetOutputIdentity is used.");
        }

        return stats;
    }
}
