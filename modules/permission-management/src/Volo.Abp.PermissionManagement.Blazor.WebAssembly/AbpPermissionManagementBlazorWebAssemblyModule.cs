using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpPermissionManagementBlazorModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpPermissionManagementHttpApiClientModule)
    )]
    public class AbpPermissionManagementBlazorWebAssemblyModule : AbpModule
    {
    }
}
