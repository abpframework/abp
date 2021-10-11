using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureValueCacheItemInvalidator :
        ILocalEventHandler<EntityChangedEventData<FeatureValue>>,
        ITransientDependency
    {
        protected IDistributedCache<FeatureValueCacheItem> Cache { get; }

        public FeatureValueCacheItemInvalidator(IDistributedCache<FeatureValueCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<FeatureValue> eventData)
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
            return FeatureValueCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
