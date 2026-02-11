using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class MvcCachedApplicationConfigurationClientHelper : ITransientDependency
{
    protected IDistributedCache<MvcCachedApplicationVersionCacheItem> ApplicationVersionCache { get; }

    public MvcCachedApplicationConfigurationClientHelper(IDistributedCache<MvcCachedApplicationVersionCacheItem> applicationVersionCache)
    {
        ApplicationVersionCache = applicationVersionCache;
    }

    public virtual async Task<string> CreateCacheKeyAsync(Guid? userId)
    {
        var appVersion = await ApplicationVersionCache.GetOrAddAsync(MvcCachedApplicationVersionCacheItem.CacheKey,
                             () => Task.FromResult(new MvcCachedApplicationVersionCacheItem(Guid.NewGuid().ToString("N")))) ??
                         new MvcCachedApplicationVersionCacheItem(Guid.NewGuid().ToString("N"));
        var userKey = userId?.ToString("N") ?? "Anonymous";
        return $"ApplicationConfiguration_{appVersion.Version}_{userKey}_{CultureInfo.CurrentUICulture.Name}";
    }
}
