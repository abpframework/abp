using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpEfCoreTestSecondContextModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<SecondDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            context.Services.AddAbpDbContext<ThirdDbContext.ThirdDbContext>(options =>
            {
                options.AddDefaultRepositories<IThirdDbContext>();
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
}