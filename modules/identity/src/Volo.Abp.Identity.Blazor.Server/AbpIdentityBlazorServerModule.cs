using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.Server;

namespace Volo.Abp.Identity.Blazor.Server;

[DependsOn(
    typeof(AbpIdentityBlazorModule),
    typeof(AbpPermissionManagementBlazorServerModule)
)]
public class AbpIdentityBlazorServerModule : AbpModule
{
}
