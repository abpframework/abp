using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule)
)]
public class AbpAspNetCoreComponentsWebBasicThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<BasicTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = BasicTheme.Name;
            }
        });
    }
}