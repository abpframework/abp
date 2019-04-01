using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.Caching
{
    [DependsOn(typeof(AbpThreadingModule))]
    [DependsOn(typeof(AbpSerializationModule))]
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class AbpCachingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();
            context.Services.AddDistributedMemoryCache();

            context.Services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));

            context.Services.Configure<CacheOptions>(cacheOptions =>
            {
                cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
            });
        }
    }
}
