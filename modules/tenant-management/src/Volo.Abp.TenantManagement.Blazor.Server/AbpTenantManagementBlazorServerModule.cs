using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.FeatureManagement.Blazor.Server;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.Server
{
    [DependsOn(
        typeof(AbpTenantManagementBlazorModule),
        typeof(AbpFeatureManagementBlazorServerModule)
        )]
    public class AbpTenantManagementBlazorServerModule : AbpModule
    {

    }
}
