using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(AbpFeaturesModule),
    typeof(AbpCachingModule)
    )]
public class AbpFeatureManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FeatureManagementOptions>(options =>
        {
            options.Providers.Add<DefaultValueFeatureManagementProvider>();
            options.Providers.Add<ConfigurationFeatureManagementProvider>();
            options.Providers.Add<EditionFeatureManagementProvider>();

            //TODO: Should be moved to the Tenant Management module
            options.Providers.Add<TenantFeatureManagementProvider>();
            options.ProviderPolicies[TenantFeatureValueProvider.ProviderName] = "AbpTenantManagement.Tenants.ManageFeatures";
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("AbpFeatureManagement", typeof(AbpFeatureManagementResource));
        });

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<FeatureManagementOptions>(options =>
            {
                options.SaveStaticFeaturesToDatabase = false;
                options.IsDynamicFeatureStoreEnabled = false;
            });
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var initializer = rootServiceProvider.GetRequiredService<FeatureDynamicInitializer>();
        await initializer.InitializeAsync(true, _cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
