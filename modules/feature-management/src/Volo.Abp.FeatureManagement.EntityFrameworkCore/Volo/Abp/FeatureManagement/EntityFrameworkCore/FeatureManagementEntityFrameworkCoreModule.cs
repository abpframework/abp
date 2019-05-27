using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(FeatureManagementDomainModule),
        typeof(EntityFrameworkCoreModule)
    )]
    public class FeatureManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FeatureManagementDbContext>(options =>
            {
                options.AddDefaultRepositories<IFeatureManagementDbContext>();

                options.AddRepository<FeatureValue, EfCoreFeatureValueRepository>();
            });
        }
    }
}