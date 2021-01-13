using Volo.Abp.AspNetCore.Components.UI.Theming.Routing;
using Volo.Abp.AspNetCore.Components.UI.Theming.Toolbars;
using Volo.Abp.AspNetCore.Components.UI.Theming.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.UI.BasicTheme.WebAssembly
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
        )]
    public class AbpAspNetCoreComponentsWebAssemblyBasicThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpAspNetCoreComponentsWebAssemblyBasicThemeModule).Assembly);
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new BasicThemeToolbarContributor());
            });
        }
    }
}
