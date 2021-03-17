using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.Server
{
    [DependsOn(
        typeof(AbpTenantManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsServerThemingModule)
        )]
    public class AbpTenantManagementBlazorServerModule : AbpModule
    {
        
    }
}