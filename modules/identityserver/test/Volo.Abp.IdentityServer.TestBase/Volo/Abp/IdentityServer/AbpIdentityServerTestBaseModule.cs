using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpIdentityServerDomainModule)
    )]
    public class AbpIdentityServerTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowPermissionChecker();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                //var dataSeeder = scope.ServiceProvider.GetRequiredService<IIdentityServerDataSeeder>();
                //AsyncHelper.RunSync(() => dataSeeder.SeedAsync("1q2w3E*"));

                scope.ServiceProvider
                    .GetRequiredService<AbpIdentityServerTestDataBuilder>()
                    .Build();
            }
        }
    }
}
