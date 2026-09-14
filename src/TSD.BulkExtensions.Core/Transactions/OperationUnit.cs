using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TSD.BulkExtensions.Transactions;

/// <summary>
/// The open connection for one operation plus a transaction of our own when the caller has none, so the statements and
/// the write-back of generated values are atomic. Disposing without <see cref="CommitAsync"/> rolls back.
/// </summary>
public sealed class OperationUnit : IAsyncDisposable
{
    private readonly bool _isAsync;
    private IDbContextTransaction? _transaction;

    private OperationUnit(ConnectionScope scope, IDbContextTransaction? transaction, bool isAsync)
    {
        Scope = scope;
        _transaction = transaction;
        _isAsync = isAsync;
    }

    /// <summary>The open connection and ambient transaction.</summary>
    public ConnectionScope Scope { get; }

    /// <summary>True when this unit opened the transaction (no ambient EF or System.Transactions transaction existed).</summary>
    public bool OwnsTransaction => _transaction is not null;

    /// <summary>Begins a transaction when none is ambient, then opens the connection.</summary>
    public static async Task<OperationUnit> BeginAsync(DbContext context, bool isAsync, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        IDbContextTransaction? transaction = null;
        if (context.Database.CurrentTransaction is null && System.Transactions.Transaction.Current is null)
        {
            transaction = isAsync
                ? await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false)
                : context.Database.BeginTransaction();
        }

        var scope = await ConnectionScope.OpenAsync(context, isAsync, cancellationToken).ConfigureAwait(false);
        return new OperationUnit(scope, transaction, isAsync);
    }

    /// <summary>Commits the owned transaction, if any.</summary>
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return;
        }

        if (_isAsync)
        {
            await _transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            _transaction.Commit();
        }
    }

    /// <summary>Releases the connection, then the transaction (rolling it back if it was never committed).</summary>
    public async ValueTask DisposeAsync()
    {
        await Scope.DisposeAsync(_isAsync).ConfigureAwait(false);

        if (_transaction is not null)
        {
            var transaction = _transaction;
            _transaction = null;
            if (_isAsync)
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
