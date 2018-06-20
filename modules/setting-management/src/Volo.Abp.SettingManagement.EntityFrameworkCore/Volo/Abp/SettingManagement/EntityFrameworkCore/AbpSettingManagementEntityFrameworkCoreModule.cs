using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [DependsOn(typeof(AbpSettingManagementDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpSettingManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<AbpSettingManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ISettingManagementDbContext>();

                options.AddRepository<Setting, EfCoreSettingRepository>();
            });

            services.AddAssemblyOf<AbpSettingManagementEntityFrameworkCoreModule>();
        }
    }
}
