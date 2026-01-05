using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class MvcCurrentApplicationConfigurationCacheResetEventHandler :
    ILocalEventHandler<CurrentApplicationConfigurationCacheResetEventData>,
    ITransientDependency
{
    protected IDistributedCache<ApplicationConfigurationDto> Cache { get; }
    protected IDistributedCache<MvcCachedApplicationVersionCacheItem> ApplicationVersionCache { get; }
    protected MvcCachedApplicationConfigurationClientHelper CacheHelper { get; }

    public MvcCurrentApplicationConfigurationCacheResetEventHandler(
        IDistributedCache<ApplicationConfigurationDto> cache,
        IDistributedCache<MvcCachedApplicationVersionCacheItem> applicationVersionCache,
        MvcCachedApplicationConfigurationClientHelper cacheHelper)
    {
        Cache = cache;
        ApplicationVersionCache = applicationVersionCache;
        CacheHelper = cacheHelper;
    }

    public virtual async Task HandleEventAsync(CurrentApplicationConfigurationCacheResetEventData eventData)
    {
        if (eventData.UserId.HasValue)
        {
            await Cache.RemoveAsync(await CacheHelper.CreateCacheKeyAsync(eventData.UserId));
        }
        else
        {
            await ApplicationVersionCache.RemoveAsync(MvcCachedApplicationVersionCacheItem.CacheKey);
        }
    }
}
