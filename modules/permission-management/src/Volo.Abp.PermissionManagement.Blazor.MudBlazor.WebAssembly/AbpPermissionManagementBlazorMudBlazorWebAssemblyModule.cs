using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor.MudBlazor.WebAssembly;

[DependsOn(
    typeof(AbpPermissionManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule),
    typeof(AbpPermissionManagementHttpApiClientModule)
)]
public class AbpPermissionManagementBlazorMudBlazorWebAssemblyModule : AbpModule
{
}
