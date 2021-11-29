using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.Blazor.Server
{
    [DependsOn(
        typeof(AbpSettingManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
    public class AbpSettingManagementBlazorServerModule : AbpModule
    {
    }
}
