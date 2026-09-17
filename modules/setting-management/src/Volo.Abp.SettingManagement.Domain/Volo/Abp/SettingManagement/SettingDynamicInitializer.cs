using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.SettingManagement;

public class SettingDynamicInitializer : ITransientDependency
{
    public ILogger<SettingDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    public SettingDynamicInitializer(IServiceProvider serviceProvider)
    {
        Logger = NullLogger<SettingDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider
            .GetRequiredService<IOptions<SettingManagementOptions>>()
            .Value;

        if (!options.SaveStaticSettingsToDatabase && !options.IsDynamicSettingStoreEnabled)
        {
            return Task.CompletedTask;
        }

        if (runInBackground)
        {
            var applicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
            Task.Run(async () =>
            {
                if (cancellationToken == default && applicationLifetime?.ApplicationStopping != null)
                {
                    cancellationToken = applicationLifetime.ApplicationStopping;
                }
                await ExecuteInitializationAsync(options, cancellationToken);
            }, cancellationToken);

            return Task.CompletedTask;
        }

        return ExecuteInitializationAsync(options, cancellationToken);
    }

    protected virtual async Task ExecuteInitializationAsync(
        SettingManagementOptions options,
        CancellationToken cancellationToken)
    {
        try
        {
            var cancellationTokenProvider = ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            using (cancellationTokenProvider.Use(cancellationToken))
            {
                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await SaveStaticSettingsToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicSettingsAsync(options);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticSettingsToDatabaseAsync(
        SettingManagementOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.SaveStaticSettingsToDatabase)
        {
            return;
        }

        var staticSettingSaver = ServiceProvider.GetRequiredService<IStaticSettingSaver>();

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                8,
                retryAttempt => TimeSpan.FromSeconds(
                    Volo.Abp.RandomHelper.GetRandom(
                        (int)Math.Pow(2, retryAttempt) * 8,
                        (int)Math.Pow(2, retryAttempt) * 12)
                )
            )
            .ExecuteAsync(async _ =>
            {
                try
                {
                    await staticSettingSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicSettingsAsync(
        SettingManagementOptions options)
    {
        if (!options.IsDynamicSettingStoreEnabled)
        {
            return;
        }

        var dynamicSettingDefinitionStore = ServiceProvider.GetRequiredService<IDynamicSettingDefinitionStore>();

        try
        {
            // Pre-cache settings, so first request doesn't wait
            await dynamicSettingDefinitionStore.GetAllAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw; // It will be cached in Initialize()
        }
    }
}
