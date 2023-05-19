using Microsoft.Extensions.Caching.Distributed;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace Volo.Abp.Caching;

[DependsOn(typeof(AbpCachingModule))]
public class AbpCachingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(option =>
        {
            option.CacheConfigurators.Add(cacheName =>
            {
                if (cacheName == CacheNameAttribute.GetCacheName(typeof(Sail.Testing.Caching.PersonCacheItem)))
                {
                    return new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpiration = DateTime.Parse("2099-01-01 12:00:00")
                    };
                }

                return null;
            });

            option.GlobalCacheEntryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(20));
        });

        context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache, TestMemoryDistributedCache>());
    }
}
