using Volo.Abp.AspNetCore.Components.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor
{
    [DependsOn(
        typeof( AbpAspNetCoreComponentsWebAssemblyThemingModule ),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementHttpApiClientModule)
        )]
    public class AbpPermissionManagementBlazorModule : AbpModule
    {

    }
}
