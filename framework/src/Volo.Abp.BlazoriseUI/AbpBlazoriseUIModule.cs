using Blazorise;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlazoriseUI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpBlazoriseUIModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureBlazorise(context);
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        context.Services.AddBlazorise(options =>
        {
            options.Debounce = true;
            options.DebounceInterval = 800;
        });

        context.Services.AddSingleton(typeof(AbpBlazorMessageLocalizerHelper<>));
    }
}
