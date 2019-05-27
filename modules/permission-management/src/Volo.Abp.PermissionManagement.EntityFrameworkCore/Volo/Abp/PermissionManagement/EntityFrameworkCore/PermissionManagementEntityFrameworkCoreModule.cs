using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore
{
    [DependsOn(typeof(PermissionManagementDomainModule))]
    [DependsOn(typeof(EntityFrameworkCoreModule))]
    public class PermissionManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PermissionManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IPermissionManagementDbContext>();

                options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
            });
        }
    }
}
