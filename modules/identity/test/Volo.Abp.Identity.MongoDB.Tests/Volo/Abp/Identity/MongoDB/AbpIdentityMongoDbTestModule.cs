using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    [DependsOn(
        typeof(AbpIdentityTestBaseModule),
        typeof(AbpPermissionManagementMongoDbModule),
        typeof(AbpIdentityMongoDbModule)
        )]
    public class AbpIdentityMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(IServiceCollection services)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _mongoDbRunner.ConnectionString;
            });

            services.AddAssemblyOf<AbpIdentityMongoDbTestModule>();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _mongoDbRunner.Dispose();
        }
    }
}
