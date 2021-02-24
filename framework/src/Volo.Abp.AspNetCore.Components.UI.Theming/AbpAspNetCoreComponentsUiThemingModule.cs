using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.UI.Theming
{
    [DependsOn(
        typeof(AbpBlazoriseUIModule),
        typeof(AbpUiNavigationModule)
        )]
    public class AbpAspNetCoreComponentsUiThemingModule : AbpModule
    {
        
    }
}