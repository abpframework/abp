using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories
{
    public abstract class BasicRepositoryBase<TEntity> :
        IBasicRepository<TEntity>,
        IServiceProviderAccessor,
        IUnitOfWorkEnabled
        where TEntity : class, IEntity
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

        public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        public IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

        public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

        public ICancellationTokenProvider CancellationTokenProvider => LazyServiceProvider.LazyGetService<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

        protected BasicRepositoryBase()
        {

        }

        public abstract Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(entity, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }

        protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (UnitOfWorkManager?.Current != null)
            {
                return UnitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
            }

            return Task.CompletedTask;
        }

        public abstract Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }

        public abstract Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }

        public abstract Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

        public abstract Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

        public abstract Task<long> GetCountAsync(CancellationToken cancellationToken = default);

        public abstract Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);

        protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return CancellationTokenProvider.FallbackToProvider(preferredValue);
        }
    }

    public abstract class BasicRepositoryBase<TEntity, TKey> : BasicRepositoryBase<TEntity>, IBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public abstract Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

        public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                await DeleteAsync(id, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }
    }
}
