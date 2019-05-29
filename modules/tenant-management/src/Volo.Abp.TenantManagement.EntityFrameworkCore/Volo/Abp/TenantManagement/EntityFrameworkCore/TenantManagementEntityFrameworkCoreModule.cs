using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [DependsOn(typeof(TenantManagementDomainModule))]
    [DependsOn(typeof(EntityFrameworkCoreModule))]
    public class TenantManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TenantManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementDbContext>();
            });
        }
    }
}
