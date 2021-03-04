using Volo.Abp.AspNetCore.Components.UI.Theming.Routing;
using Volo.Abp.AspNetCore.Components.UI.Theming.Toolbars;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.UI.BasicTheme.Server
{
    public class AbpAspNetCoreComponentsServerBasicTheme : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpAspNetCoreComponentsServerBasicTheme).Assembly);
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new BasicThemeToolbarContributor());
            });
        }
    }
}
