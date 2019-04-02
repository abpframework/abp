using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace BaseManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(BaseManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BaseManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BaseManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IBaseManagementDbContext>(true);
            });
        }
    }
}