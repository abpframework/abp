using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDb
{
    [DependsOn(
        typeof(AbpPermissionManagementMongoDbModule),
        typeof(AbpPermissionManagementTestBaseModule))]
    public class AbpPermissionManagementMongoDbTestModule : AbpModule
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
