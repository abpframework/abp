using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AutofacModule),
        typeof(TestBaseModule),
        typeof(AuthorizationModule),
        typeof(FeatureManagementDomainModule)
        )]
    public class FeatureManagementTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<FeatureManagementOptions>(options =>
            {
                //TODO: Any value can pass. After completing the permission unit test, look at it again.
                options.ProviderPolicies[EditionFeatureValueProvider.ProviderName] = EditionFeatureValueProvider.ProviderName;
            });
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<FeatureManagementTestDataBuilder>()
                    .Build();
            }
        }
    }
}
