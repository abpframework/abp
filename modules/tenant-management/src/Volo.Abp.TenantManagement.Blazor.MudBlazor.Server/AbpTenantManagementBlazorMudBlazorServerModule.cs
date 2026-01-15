using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.MudBlazor.Server;

[DependsOn(
    typeof(AbpTenantManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule)
)]
public class AbpTenantManagementBlazorMudBlazorServerModule : AbpModule
{
}
