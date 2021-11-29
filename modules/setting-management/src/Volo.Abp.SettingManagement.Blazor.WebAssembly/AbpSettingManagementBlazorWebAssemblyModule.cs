using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpSettingManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpSettingManagementHttpApiClientModule)
    )]
    public class AbpSettingManagementBlazorWebAssemblyModule : AbpModule
    {
    }
}
