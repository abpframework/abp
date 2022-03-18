using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.WebAssembly;

namespace Volo.Abp.Identity.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpIdentityBlazorModule),
    typeof(AbpPermissionManagementBlazorWebAssemblyModule),
    typeof(AbpIdentityHttpApiClientModule)
)]
public class AbpIdentityBlazorWebAssemblyModule : AbpModule
{
}
