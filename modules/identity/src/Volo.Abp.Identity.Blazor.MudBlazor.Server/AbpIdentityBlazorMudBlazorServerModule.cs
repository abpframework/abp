using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.MudBlazor.Server;

namespace Volo.Abp.Identity.Blazor.MudBlazor.Server;

[DependsOn(
    typeof(AbpIdentityBlazorMudBlazorModule),
    typeof(AbpPermissionManagementBlazorMudBlazorServerModule)
)]
public class AbpIdentityBlazorMudBlazorServerModule : AbpModule
{
}
