using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpFeaturesModule)
    )]
    public class AbpFeatureManagementBlazorModule : AbpModule
    {
        
    }
}