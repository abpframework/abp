using Mongo2Go;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement.MongoDB;
using Volo.Abp.Modularity;

namespace Abp.FeatureManagement.MongoDB
{
    [DependsOn(
        typeof(AbpFeatureManagementTestBaseModule),
        typeof(AbpFeatureManagementMongoDbModule)
        )]
    public class AbpFeatureManagementMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}