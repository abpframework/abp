using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories
{
    public abstract class RepositoryBase<TEntity> : BasicRepositoryBase<TEntity>, IRepository<TEntity>, IUnitOfWorkManagerAccessor
        where TEntity : class, IEntity
    {
        [Obsolete("This method will be removed in future versions.")]
        public virtual Type ElementType => GetQueryable().ElementType;

        [Obsolete("This method will be removed in future versions.")]
        public virtual Expression Expression => GetQueryable().Expression;

        [Obsolete("This method will be removed in future versions.")]
        public virtual IQueryProvider Provider => GetQueryable().Provider;

        [Obsolete("Use WithDetailsAsync method.")]
        public virtual IQueryable<TEntity> WithDetails()
        {
            return GetQueryable();
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public virtual IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetQueryable();
        }

        public virtual Task<IQueryable<TEntity>> WithDetailsAsync()
        {
            return GetQueryableAsync();
        }

        public virtual Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetQueryableAsync();
        }

        [Obsolete("This method will be removed in future versions.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Obsolete("This method will be removed in future versions.")]
        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }

        [Obsolete("Use GetQueryableAsync method.")]
        protected abstract IQueryable<TEntity> GetQueryable();

        public abstract Task<IQueryable<TEntity>> GetQueryableAsync();

        public abstract Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(predicate, includeDetails, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity));
            }

            return entity;
        }

        public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);

        protected virtual TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
            where TQueryable : IQueryable<TEntity>
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<ISoftDelete>(), e => ((ISoftDelete)e).IsDeleted == false);
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                var tenantId = CurrentTenant.Id;
                query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<IMultiTenant>(), e => ((IMultiTenant)e).TenantId == tenantId);
            }

            return query;
        }
    }

    public abstract class RepositoryBase<TEntity, TKey> : RepositoryBase<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public abstract Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

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
