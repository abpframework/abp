using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(SettingManagementDomainModule),
        typeof(EntityFrameworkCoreModule)
        )]
    public class SettingManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SettingManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<ISettingManagementDbContext>();

                options.AddRepository<Setting, EfCoreSettingRepository>();
            });
        }
    }
}
