using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFeatureManagementHttpApiClientModule)
    )]
    public class AbpFeatureManagementBlazorModule : AbpModule
    {
        
    }
}