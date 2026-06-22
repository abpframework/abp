using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionGrantCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<ResourcePermissionGrant>>,
    ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    protected IDistributedCache<ResourcePermissionGrantCacheItem> Cache { get; }

    public ResourcePermissionGrantCacheItemInvalidator(IDistributedCache<ResourcePermissionGrantCacheItem> cache, ICurrentTenant currentTenant)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<ResourcePermissionGrant> eventData)
    {
        var cacheKey = CalculateCacheKey(
            eventData.Entity.Name,
            eventData.Entity.ResourceName,
            eventData.Entity.ResourceKey,
            eventData.Entity.ProviderName,
            eventData.Entity.ProviderKey
        );

        using (CurrentTenant.Change(eventData.Entity.TenantId))
        {
            await Cache.RemoveAsync(cacheKey, considerUow: true);
        }
    }

    protected virtual string CalculateCacheKey(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return ResourcePermissionGrantCacheItem.CalculateCacheKey(name, resourceName, resourceKey, providerName, providerKey);
    }
}
