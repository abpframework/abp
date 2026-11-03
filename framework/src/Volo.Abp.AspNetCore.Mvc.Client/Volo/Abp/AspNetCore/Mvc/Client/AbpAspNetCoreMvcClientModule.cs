using System;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                options.ApplicationConfigurationDtoCacheAbsoluteExpiration = TimeSpan.FromSeconds(5);
            });
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PostConfigureAll<CookieAuthenticationOptions>(cookieOptions => cookieOptions.ValidateRemoteDynamicClaims());
    }
}
