using System;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.Users.MongoDB
{
    [DependsOn(
        typeof(AbpUsersMongoDbModule),
        typeof(AbpUsersTestsSharedModule)
        )]
    public class AbpUsersMongoDbTestModule : AbpModule
    {
        private static readonly MongoDbRunner MongoDbRunner = MongoDbRunner.Start();

        public override void ConfigureServices(IServiceCollection services)
        {
            var connectionString = MongoDbRunner.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });

            services.AddAssemblyOf<AbpUsersMongoDbTestModule>();
        }
    }
}
