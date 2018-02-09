using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions.EntityFrameworkCore
{
    [DependsOn(typeof(AbpPermissionsDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpPermissionsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<AbpPermissionsDbContext>(options =>
            {
                options.AddDefaultRepositories<IAbpPermissionsDbContext>();

                options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
            });

            services.AddAssemblyOf<AbpPermissionsEntityFrameworkCoreModule>();
        }
    }
}
