using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.FeatureManagement.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.Blazor.MudBlazor.WebAssembly;

[DependsOn(
    typeof(AbpTenantManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule),
    typeof(AbpFeatureManagementBlazorMudBlazorWebAssemblyModule),
    typeof(AbpTenantManagementHttpApiClientModule)
)]
public class AbpTenantManagementBlazorMudBlazorWebAssemblyModule : AbpModule
{
}
