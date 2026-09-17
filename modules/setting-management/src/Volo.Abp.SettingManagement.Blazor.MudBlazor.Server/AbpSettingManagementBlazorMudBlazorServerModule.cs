using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor.Server;

[DependsOn(
    typeof(AbpSettingManagementBlazorMudBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule)
)]
public class AbpSettingManagementBlazorMudBlazorServerModule : AbpModule
{
}
