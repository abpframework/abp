using Blazorise;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlazoriseLib
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyModule)
        )]
    public class AbpBlazoriseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureBlazorise(context);
        }

        private void ConfigureBlazorise(ServiceConfigurationContext context)
        {
            context.Services
                .AddBlazorise();
        }
    }
}
