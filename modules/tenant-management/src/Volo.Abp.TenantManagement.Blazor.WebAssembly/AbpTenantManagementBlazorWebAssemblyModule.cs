using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.FeatureManagement.Blazor.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpTenantManagementBlazorModule),
        typeof(AbpFeatureManagementBlazorWebAssemblyModule),
        typeof(AbpTenantManagementHttpApiClientModule)
        )]
    public class AbpTenantManagementBlazorWebAssemblyModule : AbpModule
    {

    }
}
