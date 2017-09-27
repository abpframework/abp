using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpEfCoreTestSecondContextModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpEfCoreTestSecondContextModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<SecondContextTestDataBuilder>()
                    .Build();
            }
        }
    }
}