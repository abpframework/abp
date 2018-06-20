using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.MongoDB
{
    [DependsOn(
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpSettingManagementTestBaseModule)
        )]
    public class AbpSettingManagementMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(IServiceCollection services)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });

            services.AddAssemblyOf<AbpSettingManagementMongoDbTestModule>();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}
