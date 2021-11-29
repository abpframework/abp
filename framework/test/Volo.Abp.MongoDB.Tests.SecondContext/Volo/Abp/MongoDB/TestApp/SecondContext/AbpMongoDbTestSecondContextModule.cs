using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB.TestApp.FourthContext;
using Volo.Abp.MongoDB.TestApp.ThirdDbContext;
using Volo.Abp.Threading;

namespace Volo.Abp.MongoDB.TestApp.SecondContext;

[DependsOn(typeof(AbpMongoDbModule))]
public class AbpMongoDbTestSecondContextModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<SecondDbContext>(options =>
        {
            options.AddDefaultRepositories();
        });

        context.Services.AddMongoDbContext<ThirdDbContext.ThirdDbContext>(options =>
        {
            options.AddDefaultRepositories<IThirdDbContext>();
        });

        context.Services.AddMongoDbContext<FourthDbContext>(options =>
        {
            options.AddDefaultRepositories<IFourthDbContext>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<SecondContextTestDataBuilder>()
                .BuildAsync());
        }
    }
}
