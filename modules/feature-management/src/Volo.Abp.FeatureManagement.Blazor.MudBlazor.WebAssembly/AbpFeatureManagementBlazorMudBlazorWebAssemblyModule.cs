using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor.MudBlazor.WebAssembly;

[DependsOn(
    typeof(AbpFeatureManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule),
    typeof(AbpFeatureManagementHttpApiClientModule)
)]
public class AbpFeatureManagementBlazorMudBlazorWebAssemblyModule : AbpModule
{
}
