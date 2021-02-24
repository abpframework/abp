using Volo.Abp.AspNetCore.Components.UI.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.UI.BasicTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsUiThemingModule)
        )]
    public class AbpAspNetCoreComponentsUiBasicThemeModule : AbpModule
    {
        
    }
}