using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.HttpApi
{
    [DependsOn(
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class AbpPermissionManagementHttpApiModule : AbpModule
    {

    }
}
