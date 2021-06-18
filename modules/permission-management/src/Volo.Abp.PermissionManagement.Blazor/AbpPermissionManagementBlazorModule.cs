using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebThemingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementApplicationContractsModule)
        )]
    public class AbpPermissionManagementBlazorModule : AbpModule
    {

    }
}
