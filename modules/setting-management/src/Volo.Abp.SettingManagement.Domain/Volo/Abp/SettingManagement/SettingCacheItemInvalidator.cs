using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.Abp.SettingManagement
{
    public class SettingCacheItemInvalidator : ILocalEventHandler<EntityChangedEventData<Setting>>, ITransientDependency
    {
        protected IDistributedCache<SettingCacheItem> Cache { get; }

        public SettingCacheItemInvalidator(IDistributedCache<SettingCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<Setting> eventData)
        {
            var cacheKey = CalculateCacheKey(
                eventData.Entity.Name,
                eventData.Entity.ProviderName,
                eventData.Entity.ProviderKey
            );

            await Cache.RemoveAsync(cacheKey, considerUow: true);
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return SettingCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
