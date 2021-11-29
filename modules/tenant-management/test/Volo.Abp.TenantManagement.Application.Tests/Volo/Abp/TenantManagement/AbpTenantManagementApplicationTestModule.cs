using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Volo.Abp.TenantManagement;

[DependsOn(
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpTenantManagementEntityFrameworkCoreTestModule))]
public class AbpTenantManagementApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }
}
