using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsUiThemingModule)
        )]
    public class AbpAspNetCoreComponentsUiBasicThemeModule : AbpModule
    {
        
    }
}