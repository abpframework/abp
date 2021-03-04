using Volo.Abp.AspNetCore.Components.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsUiThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFeatureManagementApplicationContractsModule)
    )]
    public class AbpFeatureManagementBlazorModule : AbpModule
    {
        
    }
}