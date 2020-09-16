using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementHttpApiClientModule)
        )]
    public class AbpPermissionManagementBlazorModule : AbpModule
    {

    }
}
