using Volo.Abp.AutoMapper;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.Blazor
{
    [DependsOn(
        typeof(AbpBlazoriseUIModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementHttpApiClientModule)
        )]
    public class AbpPermissionManagementBlazorModule : AbpModule
    {

    }
}
