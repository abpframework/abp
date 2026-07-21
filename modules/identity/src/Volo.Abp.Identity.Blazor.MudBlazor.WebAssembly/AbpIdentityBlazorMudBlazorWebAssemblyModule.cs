using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.MudBlazor.WebAssembly;

namespace Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly;

[DependsOn(
    typeof(AbpIdentityBlazorMudBlazorModule),
    typeof(AbpPermissionManagementBlazorMudBlazorWebAssemblyModule),
    typeof(AbpIdentityHttpApiClientModule)
)]
public class AbpIdentityBlazorMudBlazorWebAssemblyModule : AbpModule
{
}
