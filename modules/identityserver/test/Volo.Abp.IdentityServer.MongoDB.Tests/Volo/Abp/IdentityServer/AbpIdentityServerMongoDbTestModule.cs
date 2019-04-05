using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.IdentityServer.MongoDB;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{

    [DependsOn(
        typeof(AbpIdentityServerTestBaseModule),
        typeof(AbpIdentityServerMongoDbModule),
        typeof(AbpIdentityMongoDbModule)
    )]
    public class AbpIdentityServerMongoDbTestModule : AbpModule
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
