using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(IdentityEntityFrameworkCoreTestModule),
        typeof(IdentityTestBaseModule),
        typeof(PermissionManagementDomainIdentityModule)
        )]
    public class IdentityDomainTestModule : AbpModule
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
                    .GetRequiredService<TestPermissionDataBuilder>()
                    .Build();
            }
        }
    }
}
