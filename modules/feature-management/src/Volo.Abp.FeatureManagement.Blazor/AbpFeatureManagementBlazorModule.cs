using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFeatureManagementApplicationContractsModule)
    )]
    public class AbpFeatureManagementBlazorModule : AbpModule
    {
        
    }
}