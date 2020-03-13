using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpAuthorizationModule)
        )]
    public class AbpIdentityTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
                AsyncHelper.RunSync(async () =>
                {
                    await dataSeeder.SeedAsync();
                    await scope.ServiceProvider
                        .GetRequiredService<AbpIdentityTestDataBuilder>()
                        .Build();
                });
            }
        }
    }
}
