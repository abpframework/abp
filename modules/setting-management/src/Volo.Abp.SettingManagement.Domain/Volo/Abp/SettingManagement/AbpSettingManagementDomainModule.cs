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
    private Task _initializeDynamicSettingsTask;

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

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeDynamicSettings(context);
        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    public Task GetInitializeDynamicSettingsTask()
    {
        return _initializeDynamicSettingsTask ?? Task.CompletedTask;
    }

    private void InitializeDynamicSettings(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<SettingManagementOptions>>()
            .Value;

        if (!options.SaveStaticSettingsToDatabase && !options.IsDynamicSettingStoreEnabled)
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        _initializeDynamicSettingsTask = Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;

            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await SaveStaticSettingsToDatabaseAsync(options, scope, cancellationTokenProvider);

                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await PreCacheDynamicSettingsAsync(options, scope);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }

    private async static Task SaveStaticSettingsToDatabaseAsync(
        SettingManagementOptions options,
        IServiceScope scope,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticSettingsToDatabase)
        {
            return;
        }

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10))
            .ExecuteAsync(async _ =>
            {
                try
                {
                    // ReSharper disable once AccessToDisposedClosure
                    await scope
                        .ServiceProvider
                        .GetRequiredService<IStaticSettingSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    scope.ServiceProvider
                        .GetService<ILogger<AbpSettingManagementDomainModule>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationTokenProvider.Token);
    }

    private async static Task PreCacheDynamicSettingsAsync(SettingManagementOptions options, IServiceScope scope)
    {
        if (!options.IsDynamicSettingStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache settings, so first request doesn't wait
            await scope
                .ServiceProvider
                .GetRequiredService<IDynamicSettingDefinitionStore>()
                .GetAllAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            scope
                .ServiceProvider
                .GetService<ILogger<AbpSettingManagementDomainModule>>()?
                .LogException(ex);

            throw; // It will be cached in InitializeDynamicSettings
        }
    }
}
