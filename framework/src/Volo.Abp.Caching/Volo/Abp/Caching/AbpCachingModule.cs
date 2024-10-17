using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Caching.Hybrid;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Serialization;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.Caching;

[DependsOn(
    typeof(AbpThreadingModule),
    typeof(AbpSerializationModule),
    typeof(AbpUnitOfWorkModule),
    typeof(AbpMultiTenancyModule),
    typeof(AbpJsonModule))]
public class AbpCachingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMemoryCache();
        context.Services.AddDistributedMemoryCache();

        context.Services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
        context.Services.AddSingleton(typeof(IDistributedCache<,>), typeof(DistributedCache<,>));

        context.Services.AddHybridCache().AddSerializerFactory<AbpHybridCacheJsonSerializerFactory>();
        context.Services.AddSingleton(typeof(IHybridCache<>), typeof(AbpHybridCache<>));
        context.Services.AddSingleton(typeof(IHybridCache<,>), typeof(AbpHybridCache<,>));

        context.Services.Configure<AbpDistributedCacheOptions>(cacheOptions =>
        {
            cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20);
        });
    }
}
