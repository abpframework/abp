using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.HttpApi
{
    [DependsOn(
        typeof(PermissionManagementApplicationContractsModule),
        typeof(AspNetCoreMvcModule)
        )]
    public class PermissionManagementHttpApiModule : AbpModule
    {

    }
}
