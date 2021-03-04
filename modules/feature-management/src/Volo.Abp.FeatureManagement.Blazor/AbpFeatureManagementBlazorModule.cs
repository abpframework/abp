using Volo.Abp.AspNetCore.Components.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsUiThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFeatureManagementHttpApiClientModule)
    )]
    public class AbpFeatureManagementBlazorModule : AbpModule
    {
        
    }
}