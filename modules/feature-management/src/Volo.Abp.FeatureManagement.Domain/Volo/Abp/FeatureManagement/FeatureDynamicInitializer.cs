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
using Volo.Abp.Features;
using Volo.Abp.Threading;

namespace Volo.Abp.FeatureManagement;

public class FeatureDynamicInitializer : ITransientDependency
{
    public ILogger<FeatureDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    public FeatureDynamicInitializer(IServiceProvider serviceProvider)
    {
        Logger = NullLogger<FeatureDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider
            .GetRequiredService<IOptions<FeatureManagementOptions>>()
            .Value;

        if (!options.SaveStaticFeaturesToDatabase && !options.IsDynamicFeatureStoreEnabled)
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
        FeatureManagementOptions options,
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

                await SaveStaticFeaturesToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicFeaturesAsync(options);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticFeaturesToDatabaseAsync(
        FeatureManagementOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.SaveStaticFeaturesToDatabase)
        {
            return;
        }

        var staticFeatureSaver = ServiceProvider.GetRequiredService<IStaticFeatureSaver>();

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
                    await staticFeatureSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicFeaturesAsync(
        FeatureManagementOptions options)
    {
        if (!options.IsDynamicFeatureStoreEnabled)
        {
            return;
        }

        var dynamicFeatureDefinitionStore = ServiceProvider.GetRequiredService<IDynamicFeatureDefinitionStore>();

        try
        {
            // Pre-cache features, so first request doesn't wait
            await dynamicFeatureDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw; // It will be cached in Initialize()
        }
    }
}
