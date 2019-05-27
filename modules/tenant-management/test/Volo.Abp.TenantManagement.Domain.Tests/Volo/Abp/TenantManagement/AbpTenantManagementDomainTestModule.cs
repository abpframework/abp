using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(TenantManagementEntityFrameworkCoreTestModule),
        typeof(TenantManagementTestBaseModule))]
    public class SettingManagementDomainTestModule : AbpModule
    {

    }
}
