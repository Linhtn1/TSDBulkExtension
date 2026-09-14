namespace TSD.BulkExtensions;

/// <summary>
/// Runs the shared async implementation on the synchronous API. The implementation only awaits already-completed tasks
/// when called with <c>isAsync: false</c>; this guard turns a violation of that rule into an immediate error instead of a
/// sync-over-async deadlock in the caller's process.
/// </summary>
internal static class SyncBridge
{
    public static void Run(Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (!task.IsCompleted)
        {
            throw new InvalidOperationException(
                "The synchronous bulk path awaited an incomplete task. This is a bug in TSD.BulkExtensions: every await on the " +
                "isAsync == false branch must be on a completed task.");
        }

        task.GetAwaiter().GetResult();
    }
}
