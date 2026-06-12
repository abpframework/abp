using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.Modularity;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;

[DependsOn(
        typeof(AbpMudBlazorUIModule),
        typeof(AbpUiNavigationModule)
)]
public class AbpAspNetCoreComponentsWebThemingMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicLayoutComponentOptions>(options =>
        {
            options.Components.Add(typeof(AbpAuthenticationState), null);
        });
    }
}

