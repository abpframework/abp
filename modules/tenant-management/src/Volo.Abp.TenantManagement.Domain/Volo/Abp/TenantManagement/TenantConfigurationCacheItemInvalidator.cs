using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement;


public class TenantConfigurationCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<Tenant>>,
    ILocalEventHandler<EntityDeletedEventData<Tenant>>, ITransientDependency
{
    protected IDistributedCache<TenantConfigurationCacheItem> Cache { get; }

    public TenantConfigurationCacheItemInvalidator(IDistributedCache<TenantConfigurationCacheItem> cache)
    {
        Cache = cache;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<Tenant> eventData)
    {
        await ClearCacheAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEventData<Tenant> eventData)
    {
        await ClearCacheAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    protected virtual async Task ClearCacheAsync(Guid? id, string name)
    {
        await Cache.RemoveManyAsync(
            new[]
            {
                TenantConfigurationCacheItem.CalculateCacheKey(id, null),
                TenantConfigurationCacheItem.CalculateCacheKey(null, name),
            });
    }
}
