using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.IdentityServer.MongoDB;
using Volo.Abp.Modularity;

namespace Volo.Abp.IdentityServer
{

    [DependsOn(
        typeof(AbpIdentityServerTestBaseModule),
        typeof(AbpIdentityServerMongoDbModule)
    )]
    public class AbpIdentityServerMongoDbTestModule : AbpModule
    {
        private MongoDbRunner _mongoDbRunner;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _mongoDbRunner = MongoDbRunner.Start();

            context.Services.Configure<DbConnectionOptions>(options =>
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
