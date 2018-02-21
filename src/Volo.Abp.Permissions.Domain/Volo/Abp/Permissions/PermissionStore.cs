using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public class PermissionStore : IPermissionStore, ITransientDependency
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

        public PermissionStore(
            IPermissionGrantRepository permissionGrantRepository,
            IDistributedCache<PermissionGrantCacheItem> cache)
        {
            PermissionGrantRepository = permissionGrantRepository;
            Cache = cache;
        }

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
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
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
