using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.TenantManagement
{
    public class TenantStore : ITenantStore, ITransientDependency
    {
        protected ITenantRepository TenantRepository { get; }
        protected IObjectMapper<AbpTenantManagementDomainModule> ObjectMapper { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IDistributedCache<TenantCacheItem> Cache { get; }

        public TenantStore(
            ITenantRepository tenantRepository,
            IObjectMapper<AbpTenantManagementDomainModule> objectMapper,
            ICurrentTenant currentTenant,
            IDistributedCache<TenantCacheItem> cache)
        {
            TenantRepository = tenantRepository;
            ObjectMapper = objectMapper;
            CurrentTenant = currentTenant;
            Cache = cache;
        }

        public virtual async Task<TenantConfiguration> FindAsync(string name)
        {
            return (await GetCacheItemAsync(null, name)).Value;
        }

        public virtual async Task<TenantConfiguration> FindAsync(Guid id)
        {
            return (await GetCacheItemAsync(id, null)).Value;
        }

        [Obsolete("Use FindAsync method.")]
        public virtual TenantConfiguration Find(string name)
        {
            return (GetCacheItem(null, name)).Value;
        }

        [Obsolete("Use FindAsync method.")]
        public virtual TenantConfiguration Find(Guid id)
        {
            return (GetCacheItem(id, null)).Value;
        }

        protected virtual async Task<TenantCacheItem> GetCacheItemAsync(Guid? id, string name)
        {
            var cacheKey = CalculateCacheKey(id, name);

            var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            if (id.HasValue)
            {
                using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
                {
                    var tenant = await TenantRepository.FindAsync(id.Value);
                    return await SetCacheAsync(cacheKey, tenant);
                }
            }

            if (!name.IsNullOrWhiteSpace())
            {
                using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
                {
                    var tenant = await TenantRepository.FindByNameAsync(name);
                    return await SetCacheAsync(cacheKey, tenant);
                }
            }

            throw new AbpException("Both id and name can't be invalid.");
        }

        protected virtual async Task<TenantCacheItem> SetCacheAsync(string cacheKey, [CanBeNull]Tenant tenant)
        {
            var tenantConfiguration = tenant != null ? ObjectMapper.Map<Tenant, TenantConfiguration>(tenant) : null;
            var cacheItem = new TenantCacheItem(tenantConfiguration);
            await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
            return cacheItem;
        }

        [Obsolete("Use GetCacheItemAsync method.")]
        protected virtual TenantCacheItem GetCacheItem(Guid? id, string name)
        {
            var cacheKey = CalculateCacheKey(id, name);

            var cacheItem = Cache.Get(cacheKey, considerUow: true);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            if (id.HasValue)
            {
                using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
                {
                    var tenant = TenantRepository.FindById(id.Value);
                    return SetCache(cacheKey, tenant);
                }
            }

            if (!name.IsNullOrWhiteSpace())
            {
                using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
                {
                    var tenant = TenantRepository.FindByName(name);
                    return SetCache(cacheKey, tenant);
                }
            }

            throw new AbpException("Both id and name can't be invalid.");
        }

        [Obsolete("Use SetCacheAsync method.")]
        protected virtual TenantCacheItem SetCache(string cacheKey, [CanBeNull]Tenant tenant)
        {
            var tenantConfiguration = tenant != null ? ObjectMapper.Map<Tenant, TenantConfiguration>(tenant) : null;
            var cacheItem = new TenantCacheItem(tenantConfiguration);
            Cache.Set(cacheKey, cacheItem, considerUow: true);
            return cacheItem;
        }

        protected virtual string CalculateCacheKey(Guid? id, string name)
        {
            return TenantCacheItem.CalculateCacheKey(id, name);
        }
    }
}
