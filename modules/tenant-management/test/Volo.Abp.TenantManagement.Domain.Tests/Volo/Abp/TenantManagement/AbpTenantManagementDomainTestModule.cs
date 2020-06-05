using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(AbpTenantManagementEntityFrameworkCoreTestModule),
        typeof(AbpTenantManagementTestBaseModule))]
    public class AbpSettingManagementDomainTestModule : AbpModule
    {

    }
}
