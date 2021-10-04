using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.Abp.TenantManagement
{
    public class TenantCacheItemInvalidator : ILocalEventHandler<EntityChangedEventData<Tenant>>, ITransientDependency
    {
        protected IDistributedCache<TenantCacheItem> Cache { get; }

        public TenantCacheItemInvalidator(IDistributedCache<TenantCacheItem> cache)
        {
            Cache = cache;
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<Tenant> eventData)
        {
            await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(eventData.Entity.Id, null), considerUow: true);
            await Cache.RemoveAsync(TenantCacheItem.CalculateCacheKey(null, eventData.Entity.Name), considerUow: true);
        }
    }
}
