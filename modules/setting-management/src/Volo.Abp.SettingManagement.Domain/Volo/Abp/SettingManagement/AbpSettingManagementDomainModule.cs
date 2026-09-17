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
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.SettingManagement;

[DependsOn(
    typeof(AbpSettingsModule),
    typeof(AbpDddDomainModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(AbpCachingModule)
    )]
public class AbpSettingManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SettingManagementOptions>(options =>
        {
            options.Providers.Add<DefaultValueSettingManagementProvider>();
            options.Providers.Add<ConfigurationSettingManagementProvider>();
            options.Providers.Add<GlobalSettingManagementProvider>();
            options.Providers.Add<TenantSettingManagementProvider>();
            options.Providers.Add<UserSettingManagementProvider>();
        });

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<SettingManagementOptions>(options =>
            {
                options.SaveStaticSettingsToDatabase = false;
                options.IsDynamicSettingStoreEnabled = false;
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
        var initializer = rootServiceProvider.GetRequiredService<SettingDynamicInitializer>();
        await initializer.InitializeAsync(true, _cancellationTokenSource.Token);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
