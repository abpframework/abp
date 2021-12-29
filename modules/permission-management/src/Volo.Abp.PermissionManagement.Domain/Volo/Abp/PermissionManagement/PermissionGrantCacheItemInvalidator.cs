using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class PermissionGrantCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<PermissionGrant>>,
    ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

    public PermissionGrantCacheItemInvalidator(IDistributedCache<PermissionGrantCacheItem> cache, ICurrentTenant currentTenant)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
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

    protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
    {
        return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
    }
}
