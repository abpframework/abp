using System;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Entities.Caching
{
    public abstract class MultiTenancyEntityCache<TEntity, TCacheItem, TKey> :
        EntityCache<TEntity, TCacheItem, TKey>
        where TEntity : class, IEntity<TKey>, IMultiTenant
        where TCacheItem : class
    {
        protected ICurrentTenant CurrentTenant { get; }

        protected MultiTenancyEntityCache(
            IRepository<TEntity> repository,
            IDistributedCache<TCacheItem> internalCache,
            ICurrentTenant currentTenant,
            string cacheName = null) : base(repository, internalCache, cacheName)
        {
            CurrentTenant = currentTenant;
        }

        protected override string GetCacheKey(TKey id)
        {
            return GetCacheKey(id, CurrentTenant.Id);
        }

        protected override string GetCacheKey(TEntity entity)
        {
            return GetCacheKey(entity.Id, entity.TenantId);
        }

        protected virtual string GetCacheKey(TKey id, Guid? tenantId)
        {
            return id + "@" + tenantId;
        }

        public override string ToString()
        {
            return $"MultiTenancyEntityCache {CacheName}";
        }
    }
}
