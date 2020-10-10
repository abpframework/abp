using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Domain.Entities.Caching
{
    public abstract class MultiTenancyEntityCache<TEntity, TCacheItem, TKey> :
        AbstractKeyMultiTenancyEntityCache<TEntity, TCacheItem, TKey>
        where TEntity : class, IEntity<TKey>, IMultiTenant
        where TCacheItem : class
    {
        protected IRepository<TEntity, TKey> Repository { get; }

        protected MultiTenancyEntityCache(
            IRepository<TEntity, TKey> repository,
            IDistributedCache<TCacheItem> internalCache,
            ICurrentTenant currentTenant,
            string cacheName = null) : base(repository, internalCache, currentTenant)
        {
            Repository = repository;
        }

        protected override TEntity GetEntityFromDataSource(TKey key)
        {
            return AsyncHelper.RunSync(() => GetEntityFromDataSourceAsync(key));
        }

        protected override async Task<TEntity> GetEntityFromDataSourceAsync(TKey key)
        {
            return await Repository.FirstOrDefaultAsync(entity => entity.Id.Equals(key));
        }

        protected override string GetCacheKey(TKey key)
        {
            return GetCacheKey(key.ToString(), CurrentTenant.Id);
        }

        protected override string GetCacheKey(TEntity entity)
        {
            return GetCacheKey(entity.Id.ToString(), entity.TenantId);
        }
    }
}
