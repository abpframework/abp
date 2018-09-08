using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class AbpTenantManagementHttpApiModule : AbpModule
    {

    }
}