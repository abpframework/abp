using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    [DependsOn(
        typeof(FeatureManagementDomainModule),
        typeof(MongoDbModule)
        )]
    public class FeatureManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<FeatureManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<IFeatureManagementMongoDbContext>();

                options.AddRepository<FeatureValue, MongoFeatureValueRepository>();
            });
        }
    }
}
