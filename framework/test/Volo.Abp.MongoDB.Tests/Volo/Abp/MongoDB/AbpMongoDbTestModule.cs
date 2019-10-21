using System;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.MongoDB;

namespace Volo.Abp.MongoDB
{
    [DependsOn(
        typeof(AbpMongoDbModule),
        typeof(TestAppModule)
        )]
    public class AbpMongoDbTestModule : AbpModule
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

            context.Services.AddMongoDbContext<TestAppMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ITestAppMongoDbContext>();
                options.AddRepository<City, CityRepository>();
            });
        }
    }
}
