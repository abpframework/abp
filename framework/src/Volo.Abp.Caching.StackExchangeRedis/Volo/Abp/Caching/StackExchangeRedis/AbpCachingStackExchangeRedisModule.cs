using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace Volo.Abp.Caching.StackExchangeRedis;

[DependsOn(
    typeof(AbpCachingModule)
    )]
public class AbpCachingStackExchangeRedisModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var redisEnabled = configuration["Redis:IsEnabled"];
        if (redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled))
        {
            context.Services.AddStackExchangeRedisCache(options =>
            {
                var redisConfiguration = configuration["Redis:Configuration"];
                if (!redisConfiguration.IsNullOrEmpty())
                {
                    options.Configuration = redisConfiguration;
                }
            });

            context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache, AbpRedisCache>());
        }
    }
}
