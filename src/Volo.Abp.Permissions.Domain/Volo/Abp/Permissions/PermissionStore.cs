using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Permissions
{
    //TODO: Extract cache invalidate logic to another class
    public class PermissionStore : IPermissionStore, IAsyncEventHandler<EntityChangedEventData<PermissionGrant>>, ITransientDependency
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }
        protected ICurrentTenant CurrentTenant { get; }

        public PermissionStore(
            IPermissionGrantRepository permissionGrantRepository,
            IDistributedCache<PermissionGrantCacheItem> cache, 
            ICurrentTenant currentTenant)
        {
            PermissionGrantRepository = permissionGrantRepository;
            Cache = cache;
            CurrentTenant = currentTenant;
        }

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }
        
        public virtual async Task HandleEventAsync(EntityChangedEventData<PermissionGrant> eventData)
        {
            var cacheKey = CalculateCacheKey(
                eventData.Entity.Name,
                eventData.Entity.ProviderName,
                eventData.Entity.ProviderKey
            );

            using (CurrentTenant.Change(eventData.Entity.TenantId))
            {
                await Cache.RemoveAsync(cacheKey);
            }
        }

        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
            var cacheItem = await Cache.GetAsync(cacheKey);

            if (cacheItem != null)
            {
                return cacheItem;
            }

            cacheItem = new PermissionGrantCacheItem(
                name,
                await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null
            );

            await Cache.SetAsync(
                cacheKey,
                cacheItem
            );

            return cacheItem;
        }
        
        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return "pn:" + providerName + ",pk:" + providerKey + ",n:" + name;
        }
    }
}
