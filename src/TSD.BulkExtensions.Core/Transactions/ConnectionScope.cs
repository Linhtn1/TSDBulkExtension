using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

    /// <summary>Releases the connection reference taken by <see cref="OpenAsync"/>; the connection closes only when no one else holds it.</summary>
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
