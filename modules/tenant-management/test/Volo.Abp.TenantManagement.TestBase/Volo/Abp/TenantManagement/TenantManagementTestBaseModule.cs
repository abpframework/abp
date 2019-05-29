using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(TenantManagementDomainModule),
        typeof(AutofacModule),
        typeof(TestBaseModule)
        )]
    public class TenantManagementTestBaseModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<AbpTenantManagementTestDataBuilder>()
                    .Build();
            }
        }
    }
}
