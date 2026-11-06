using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.RobotsTxts;

public class RobotsTxtCacheInvalidator :
    ILocalEventHandler<EntityChangedEventData<RobotsTxt>>,
    ITransientDependency
{
    protected IDistributedCache<RobotsTxtCacheItem> Cache { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public RobotsTxtCacheInvalidator(
        IDistributedCache<RobotsTxtCacheItem> cache,
        ICurrentTenant currentTenant)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<RobotsTxt> eventData)
    {
        using (CurrentTenant.Change(eventData.Entity.TenantId))
        {
            await Cache.RemoveAsync(RobotsTxtCacheItem.CacheKey, considerUow: true);
        }
    }
}
