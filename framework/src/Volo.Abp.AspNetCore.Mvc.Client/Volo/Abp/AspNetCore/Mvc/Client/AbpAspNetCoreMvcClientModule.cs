using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client;

[DependsOn(
    typeof(AbpAspNetCoreMvcClientCommonModule),
    typeof(AbpEventBusModule)
    )]
public class AbpAspNetCoreMvcClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var abpHostEnvironment = context.Services.GetAbpHostEnvironment();
        if (abpHostEnvironment.IsDevelopment())
        {
            Configure<AbpAspNetCoreMvcClientCacheOptions>(options =>
            {
                options.TenantConfigurationCacheAbsoluteExpiration = TimeSpan.FromSeconds(5);
                options.ApplicationConfigurationDtoCacheAbsoluteExpiration = TimeSpan.FromSeconds(5);
            });
        }
    }
}
