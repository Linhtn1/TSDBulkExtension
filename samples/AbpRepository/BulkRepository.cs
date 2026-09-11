using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using TSD.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TSD.Account.EntityFrameworkCore.Repositories
{
    public class BulkRepository<TEntity, PrimaryKey> : AccountRepositoryBase<TEntity, PrimaryKey>, IBulkRepository<TEntity, PrimaryKey> where TEntity : class, IEntity<PrimaryKey>
    {
        public BulkRepository(IDbContextProvider<AccountDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        public void BulkDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkDelete(entities, bulkConfig, progress, type);
        }

        public void BulkDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkDelete(entities, bulkAction, progress, type);
        }

        public async Task BulkDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkDeleteAsync(entities, bulkAction, progress, type, cancellationToken);
        }

        public async Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkDeleteAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public void BulkInsert(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkInsert(entities, bulkConfig, progress, type);
        }
        public void BulkInsert(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkInsert(entities, bulkAction, progress, type);
        }

        public async Task BulkInsertAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkInsertAsync(entities, bulkAction, progress, type, cancellationToken);
        }

        public async Task BulkInsertAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkInsertAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public void BulkInsertOrUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkInsertOrUpdate(entities, bulkAction, progress, type);
        }

        public void BulkInsertOrUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkInsertOrUpdate(entities, bulkConfig, progress, type);
        }

        public async Task BulkInsertOrUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkInsertOrUpdateAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public async Task BulkInsertOrUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkInsertOrUpdateAsync(entities, bulkAction, progress, type, cancellationToken);
        }

        public void BulkInsertOrUpdateOrDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkInsertOrUpdateOrDelete(entities, bulkConfig, progress, type);
        }

        public void BulkInsertOrUpdateOrDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkInsertOrUpdateOrDelete(entities, bulkAction, progress, type);
        }

        public async Task BulkInsertOrUpdateOrDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkInsertOrUpdateOrDeleteAsync(entities, bulkAction, progress, type, cancellationToken);
        }

        public async Task BulkInsertOrUpdateOrDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkInsertOrUpdateOrDeleteAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public void BulkRead(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkRead(entities, bulkAction, progress, type);
        }

        public void BulkRead(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkRead(entities, bulkConfig, progress, type);
        }

        public async Task BulkReadAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkReadAsync(entities, bulkAction, progress, type, cancellationToken);
        }

        public async Task BulkReadAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkReadAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public void BulkUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkUpdate(entities, bulkConfig, progress, type);
        }

        public void BulkUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
        {
            GetContext().BulkUpdate(entities, bulkAction, progress, type);
        }

        public async Task BulkUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkUpdateAsync(entities, bulkConfig, progress, type, cancellationToken);
        }

        public async Task BulkUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().BulkUpdateAsync(entities, bulkAction, progress, type, cancellationToken);
        }

        public void Truncate(Type type = null)
        {
            GetContext().Truncate<TEntity>(type);
        }

        public async Task TruncateAsync(Type type = null, CancellationToken cancellationToken = default)
        {
            await GetContext().TruncateAsync<TEntity>(type, cancellationToken);
        }
    }
}
