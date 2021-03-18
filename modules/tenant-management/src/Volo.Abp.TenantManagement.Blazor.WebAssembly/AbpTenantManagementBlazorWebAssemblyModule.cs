using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpTenantManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpTenantManagementHttpApiClientModule)
        )]
    public class AbpTenantManagementBlazorWebAssemblyModule : AbpModule
    {
        
    }
}