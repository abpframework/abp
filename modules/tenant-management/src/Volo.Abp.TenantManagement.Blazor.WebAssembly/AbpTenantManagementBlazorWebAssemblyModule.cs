using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpTenantManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
        )]
    public class AbpTenantManagementBlazorWebAssemblyModule : AbpModule
    {
        
    }
}