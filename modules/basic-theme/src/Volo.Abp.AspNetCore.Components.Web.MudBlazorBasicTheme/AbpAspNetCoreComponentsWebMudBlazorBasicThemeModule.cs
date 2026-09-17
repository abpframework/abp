using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule)
)]
public class AbpAspNetCoreComponentsWebMudBlazorBasicThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<MudBlazorBasicTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = MudBlazorBasicTheme.Name;
            }
        });
    }
}
