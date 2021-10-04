using Volo.Abp.AspNetCore.Components.Web.BasicTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebBasicThemeModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpHttpClientIdentityModelWebAssemblyModule)
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
