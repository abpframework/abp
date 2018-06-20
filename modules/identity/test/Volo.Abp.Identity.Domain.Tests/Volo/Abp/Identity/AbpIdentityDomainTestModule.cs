using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityEntityFrameworkCoreTestModule))]
    public class AbpIdentityDomainTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<IdentityTestPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpIdentityDomainTestModule>();
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
                    .GetRequiredService<TestPermissionDataBuilder>()
                    .Build();
            }
        }
    }
}
