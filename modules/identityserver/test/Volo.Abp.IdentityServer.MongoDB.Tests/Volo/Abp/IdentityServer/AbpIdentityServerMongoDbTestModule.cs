using System;
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
        private static readonly MongoDbRunner MongoDbRunner = MongoDbRunner.Start();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbRunner.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                   Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}
