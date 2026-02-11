using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.Theming;

[DependsOn(
    typeof(AbpBlazoriseUIModule),
    typeof(AbpUiNavigationModule)
    )]
public class AbpAspNetCoreComponentsWebThemingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicLayoutComponentOptions>(options =>
        {
            options.Components.Add(typeof(AbpAuthenticationState), null);
        });
    }
}
