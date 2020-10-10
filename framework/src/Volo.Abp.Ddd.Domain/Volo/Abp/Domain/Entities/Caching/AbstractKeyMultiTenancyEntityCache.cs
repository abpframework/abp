using System;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Entities.Caching
{
    public abstract class AbstractKeyMultiTenancyEntityCache<TEntity, TCacheItem, TKey> :
        AbstractKeyEntityCache<TEntity, TCacheItem, TKey>
        where TEntity : class, IEntity, IMultiTenant
        where TCacheItem : class
    {
        protected ICurrentTenant CurrentTenant { get; }

        public AbstractKeyMultiTenancyEntityCache(
            IRepository<TEntity> repository,
            IDistributedCache<TCacheItem> internalCache,
            ICurrentTenant currentTenant,
            string cacheName = null) : base(repository, internalCache, cacheName)
        {
            CurrentTenant = currentTenant;
        }

        protected override string GetCacheKey(TEntity entity)
        {
            return GetCacheKey(string.Join("", entity.GetKeys()), entity.TenantId);
        }

        protected virtual string GetCacheKey(string key, Guid? tenantId)
        {
            return key + "@" + tenantId;
        }

        public override string ToString()
        {
            return $"MultiTenancyEntityCache {CacheName}";
        }
    }
}
