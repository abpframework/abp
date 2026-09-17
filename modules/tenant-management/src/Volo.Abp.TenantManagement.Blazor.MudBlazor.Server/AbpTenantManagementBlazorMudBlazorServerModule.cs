using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.FeatureManagement.Blazor.MudBlazor.Server;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.MudBlazor.Server;

[DependsOn(
    typeof(AbpTenantManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule),
    typeof(AbpFeatureManagementBlazorMudBlazorServerModule)
)]
public class AbpTenantManagementBlazorMudBlazorServerModule : AbpModule
{
}
