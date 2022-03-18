using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpFeatureManagementDomainModule)
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
            options.Providers.InsertBefore(typeof(TenantFeatureManagementProvider), typeof(NextTenantFeatureManagementProvider));

            //TODO: Any value can pass. After completing the permission unit test, look at it again.
            options.ProviderPolicies[EditionFeatureValueProvider.ProviderName] = EditionFeatureValueProvider.ProviderName;
        });
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            AsyncHelper.RunSync(() => scope.ServiceProvider
                .GetRequiredService<FeatureManagementTestDataBuilder>()
                .BuildAsync());
        }
    }
}
