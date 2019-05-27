using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(TenantManagementApplicationContractsModule),
        typeof(AspNetCoreMvcModule)
        )]
    public class TenantManagementHttpApiModule : AbpModule
    {

    }
}