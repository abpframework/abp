using Blazorise;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlazoriseUI
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyModule)
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
                options.DelayTextOnKeyPress = true;
            });

            context.Services.AddSingleton(typeof(AbpBlazorMessageLocalizerHelper<>));
        }
    }
}
