using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor.MudBlazor.Server;

[DependsOn(
    typeof(AbpPermissionManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule)
)]
public class AbpPermissionManagementBlazorMudBlazorServerModule : AbpModule
{
}
