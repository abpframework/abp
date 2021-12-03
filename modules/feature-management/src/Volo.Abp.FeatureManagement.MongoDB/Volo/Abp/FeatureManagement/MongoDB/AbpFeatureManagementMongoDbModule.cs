using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB;

[DependsOn(
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class AbpFeatureManagementMongoDbModule : AbpModule
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
