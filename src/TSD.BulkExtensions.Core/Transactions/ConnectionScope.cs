using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TSD.BulkExtensions.Output;

namespace TSD.BulkExtensions.Transactions;

/// <summary>
/// Opens the context's connection for the duration of a bulk operation and exposes the ambient EF transaction, if any,
/// so every statement (bulk copy, staging table, merge, drop) joins the caller's unit of work. EF reference-counts
/// <c>OpenConnection</c>/<c>CloseConnection</c>, so a connection the caller already opened stays open after disposal.
/// </summary>
public sealed class ConnectionScope : IDisposable, IAsyncDisposable
{
    private readonly DbContext _context;
    private bool _disposed;

    private ConnectionScope(DbContext context)
    {
        _context = context;
        Connection = context.Database.GetDbConnection();
        Transaction = context.Database.CurrentTransaction?.GetDbTransaction();
    }

    /// <summary>The open connection.</summary>
    public DbConnection Connection { get; }

    /// <summary>The ambient transaction, or <c>null</c> when the caller runs outside one.</summary>
    public DbTransaction? Transaction { get; }

    /// <summary>True when an EF transaction (for example an ABP UnitOfWork) is active on the context.</summary>
    public bool HasTransaction => Transaction is not null;

    /// <summary>Command timeout configured on the context (seconds), if any.</summary>
    public int? CommandTimeout => _context.Database.GetCommandTimeout();

    /// <summary>Opens the context's connection (reference-counted by EF) and captures the ambient transaction.</summary>
    public static ConnectionScope Open(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Database.OpenConnection();
        return new ConnectionScope(context);
    }

    /// <summary>Opens the context's connection (reference-counted by EF) and captures the ambient transaction.</summary>
    public static async Task<ConnectionScope> OpenAsync(DbContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        await context.Database.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        return new ConnectionScope(context);
    }

    /// <summary>Opens synchronously or asynchronously, for implementations that share one code path behind an <c>isAsync</c> flag.</summary>
    public static async Task<ConnectionScope> OpenAsync(DbContext context, bool isAsync, CancellationToken cancellationToken)
        => isAsync ? await OpenAsync(context, cancellationToken).ConfigureAwait(false) : Open(context);

    /// <summary>Creates a command on the scope's connection, enlisted in the ambient transaction and using EF's command timeout.</summary>
    public DbCommand CreateCommand(string commandText)
    {
        var command = Connection.CreateCommand();
        command.CommandText = commandText;
        command.Transaction = Transaction;
        var timeout = CommandTimeout;
        if (timeout.HasValue)
        {
            command.CommandTimeout = timeout.Value;
        }

        return command;
    }

    /// <summary>Runs a statement and returns the affected row count.</summary>
    public async Task<int> ExecuteNonQueryAsync(string sql, bool isAsync, CancellationToken cancellationToken)
    {
        using var command = CreateCommand(sql);
        return isAsync
            ? await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false)
            : command.ExecuteNonQuery();
    }

    /// <summary>Cleanup statements run in finally blocks: their own failure must never replace the exception being propagated.</summary>
    public async Task ExecuteQuietlyAsync(string sql, bool isAsync)
    {
        try
        {
            await ExecuteNonQueryAsync(sql, isAsync, CancellationToken.None).ConfigureAwait(false);
        }
        catch (DbException)
        {
            // Temp tables and session settings die with the session anyway; this also covers an already aborted transaction.
        }
        catch (InvalidOperationException)
        {
            // Connection already broken by the failing statement.
        }
    }

    /// <summary>
    /// Reads rows shaped as (row index, action code, generated columns...) into <see cref="OutputRow"/>s. The action code is
    /// the first letter of a MERGE <c>$action</c> (I/U/D) or <c>R</c> for a plain read-back.
    /// </summary>
    public async Task<List<OutputRow>> ReadOutputRowsAsync(string sql, int valueCount, bool isAsync, CancellationToken cancellationToken)
    {
        var rows = new List<OutputRow>();
        using var command = CreateCommand(sql);
        using var reader = isAsync ? await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false) : command.ExecuteReader();

        while (isAsync ? await reader.ReadAsync(cancellationToken).ConfigureAwait(false) : reader.Read())
        {
            int? index = reader.IsDBNull(0) ? null : reader.GetInt32(0);
            var actionCode = reader.GetString(1)[0];
            var action = actionCode == 'R' ? OutputAction.Read : OutputWriteBack.FromMergeAction(actionCode);
            var values = new object?[valueCount];
            for (var i = 0; i < valueCount; i++)
            {
                values[i] = reader.IsDBNull(i + 2) ? null : reader.GetValue(i + 2);
            }

            rows.Add(new OutputRow(index, action, values));
        }

        return rows;
    }

    /// <summary>Releases the connection reference taken by <see cref="Open"/>; the connection closes only when no one else holds it.</summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _context.Database.CloseConnection();
    }

    /// <summary>Releases the connection reference taken by <see cref="OpenAsync(DbContext, CancellationToken)"/>; the connection closes only when no one else holds it.</summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        await _context.Database.CloseConnectionAsync().ConfigureAwait(false);
    }

    /// <summary>Disposes synchronously or asynchronously, for implementations that share one code path behind an <c>isAsync</c> flag.</summary>
    public ValueTask DisposeAsync(bool isAsync)
    {
        if (isAsync)
        {
            return DisposeAsync();
        }

        Dispose();
        return default;
    }
}
