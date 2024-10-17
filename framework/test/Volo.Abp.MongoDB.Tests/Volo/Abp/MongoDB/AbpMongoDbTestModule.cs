using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB.TestApp.FifthContext;
using Volo.Abp.MongoDB.TestApp.SecondContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.MongoDb;
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
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });

        context.Services.AddMongoDbContext<TestAppMongoDbContext>(options =>
        {
            options.AddDefaultRepositories<ITestAppMongoDbContext>();
            options.AddRepository<City, CityRepository>();

            options.ReplaceDbContext<IThirdDbContext>();
        });

        context.Services.AddMongoDbContext<HostTestAppDbContext>(options =>
        {
            options.AddDefaultRepositories<IFifthDbContext>();
            options.ReplaceDbContext<IFifthDbContext>(MultiTenancySides.Host);
        });

        context.Services.AddMongoDbContext<TenantTestAppDbContext>(options =>
        {
            options.AddDefaultRepositories<IFifthDbContext>();
        });
    }
}
