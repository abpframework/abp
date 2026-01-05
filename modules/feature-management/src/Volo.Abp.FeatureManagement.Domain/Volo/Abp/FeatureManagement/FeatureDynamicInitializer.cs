using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
    protected IOptions<FeatureManagementOptions> Options { get; }
    [CanBeNull]
    protected IHostApplicationLifetime ApplicationLifetime { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IDynamicFeatureDefinitionStore DynamicFeatureDefinitionStore { get; }
    protected IStaticFeatureSaver StaticFeatureSaver { get; }

    public FeatureDynamicInitializer(
        IServiceProvider serviceProvider,
        IOptions<FeatureManagementOptions> options,
        ICancellationTokenProvider cancellationTokenProvider,
        IDynamicFeatureDefinitionStore dynamicFeatureDefinitionStore,
        IStaticFeatureSaver staticFeatureSaver)
    {
        Logger = NullLogger<FeatureDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
        Options = options;
        ApplicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
        CancellationTokenProvider = cancellationTokenProvider;
        DynamicFeatureDefinitionStore = dynamicFeatureDefinitionStore;
        StaticFeatureSaver = staticFeatureSaver;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = Options.Value;

        if (!options.SaveStaticFeaturesToDatabase && !options.IsDynamicFeatureStoreEnabled)
        {
            return Task.CompletedTask;
        }

        if (runInBackground)
        {
            Task.Run(async () =>
            {
                if (cancellationToken == default && ApplicationLifetime?.ApplicationStopping != null)
                {
                    cancellationToken = ApplicationLifetime.ApplicationStopping;
                }
                await ExecuteInitializationAsync(options, cancellationToken);
            }, cancellationToken);
            return Task.CompletedTask;
        }

        return ExecuteInitializationAsync(options, cancellationToken);
    }

    protected virtual async Task ExecuteInitializationAsync(FeatureManagementOptions options, CancellationToken cancellationToken)
    {
        try
        {
            using (CancellationTokenProvider.Use(cancellationToken))
            {
                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await SaveStaticFeaturesToDatabaseAsync(options, cancellationToken);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
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
                    await StaticFeatureSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicFeaturesAsync(FeatureManagementOptions options)
    {
        if (!options.IsDynamicFeatureStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache features, so first request doesn't wait
            await DynamicFeatureDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw; // It will be cached in Initialize()
        }
    }
}
