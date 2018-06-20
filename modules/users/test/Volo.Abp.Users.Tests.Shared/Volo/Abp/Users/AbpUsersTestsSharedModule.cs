using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Users
{
    [DependsOn(
        typeof(AbpUsersDomainModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpUsersTestsSharedModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpUsersTestsSharedModule>();
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
                    .GetRequiredService<AbpUsersTestDataBuilder>()
                    .Build();
            }
        }
    }
}
