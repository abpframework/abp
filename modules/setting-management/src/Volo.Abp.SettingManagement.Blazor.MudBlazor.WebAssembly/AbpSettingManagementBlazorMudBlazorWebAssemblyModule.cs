using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor.WebAssembly;

[DependsOn(
    typeof(AbpSettingManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule),
    typeof(AbpSettingManagementHttpApiClientModule)
)]
public class AbpSettingManagementBlazorMudBlazorWebAssemblyModule : AbpModule
{
}
