using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpFeatureManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
    public class AbpTenantManagementBlazorWebAssemblyModule : AbpModule
    {
    }
}
