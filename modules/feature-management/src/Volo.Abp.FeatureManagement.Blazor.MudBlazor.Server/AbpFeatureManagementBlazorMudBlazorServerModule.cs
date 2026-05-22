using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor.MudBlazor.Server;

[DependsOn(
    typeof(AbpFeatureManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule)
)]
public class AbpFeatureManagementBlazorMudBlazorServerModule : AbpModule
{
}
