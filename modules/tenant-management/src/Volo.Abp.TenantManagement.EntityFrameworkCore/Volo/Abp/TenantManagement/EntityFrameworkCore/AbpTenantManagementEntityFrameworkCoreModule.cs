using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [DependsOn(typeof(AbpTenantManagementDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpTenantManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<TenantManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementDbContext>();
            });

            services.AddAssemblyOf<AbpTenantManagementEntityFrameworkCoreModule>();
        }
    }
}
