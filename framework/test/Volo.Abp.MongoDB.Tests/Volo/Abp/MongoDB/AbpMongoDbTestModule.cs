using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB.TestApp.SecondContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.MongoDB;

namespace Volo.Abp.MongoDB;

[DependsOn(
    typeof(TestAppModule),
    typeof(AbpMongoDbTestSecondContextModule)
    )]
public class AbpMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var stringArray = MongoDbFixture.ConnectionString.Split('?');
        var connectionString = stringArray[0].EnsureEndsWith('/') +
                                   "Db_" +
                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });

        context.Services.AddMongoDbContext<TestAppMongoDbContext>(options =>
        {
            options.AddDefaultRepositories<ITestAppMongoDbContext>();
            options.AddRepository<City, CityRepository>();

            options.ReplaceDbContext<IThirdDbContext>();
        });
    }
}
