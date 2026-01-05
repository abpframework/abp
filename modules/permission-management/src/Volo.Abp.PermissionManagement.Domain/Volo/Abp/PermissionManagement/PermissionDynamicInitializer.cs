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
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.PermissionManagement;

public class PermissionDynamicInitializer : ITransientDependency
{
    public ILogger<PermissionDynamicInitializer> Logger { get; set; }

    protected IServiceProvider ServiceProvider { get; }
    protected IOptions<PermissionManagementOptions> Options { get; }
    [CanBeNull]
    protected IHostApplicationLifetime ApplicationLifetime { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IDynamicPermissionDefinitionStore DynamicPermissionDefinitionStore { get; }
    protected IStaticPermissionSaver StaticPermissionSaver { get; }

    public PermissionDynamicInitializer(
        IServiceProvider serviceProvider,
        IOptions<PermissionManagementOptions> options,
        ICancellationTokenProvider cancellationTokenProvider,
        IDynamicPermissionDefinitionStore dynamicPermissionDefinitionStore,
        IStaticPermissionSaver staticPermissionSaver)
    {
        Logger = NullLogger<PermissionDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
        Options = options;
        ApplicationLifetime = ServiceProvider.GetService<IHostApplicationLifetime>();
        CancellationTokenProvider = cancellationTokenProvider;
        DynamicPermissionDefinitionStore = dynamicPermissionDefinitionStore;
        StaticPermissionSaver = staticPermissionSaver;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = Options.Value;

        if (!options.SaveStaticPermissionsToDatabase && !options.IsDynamicPermissionStoreEnabled)
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

    protected virtual async Task ExecuteInitializationAsync(PermissionManagementOptions options, CancellationToken cancellationToken)
    {
        try
        {
            using (CancellationTokenProvider.Use(cancellationToken))
            {
                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await SaveStaticPermissionsToDatabaseAsync(options, cancellationToken);

                if (CancellationTokenProvider.Token.IsCancellationRequested)
                {
                    return;
                }

                await PreCacheDynamicPermissionsAsync(options);
            }
        }
        catch
        {
            // No need to log here since inner calls log
        }
    }

    protected virtual async Task SaveStaticPermissionsToDatabaseAsync(
        PermissionManagementOptions options,
        CancellationToken cancellationToken)
    {
        if (!options.SaveStaticPermissionsToDatabase)
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
                    await StaticPermissionSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicPermissionsAsync(PermissionManagementOptions options)
    {
        if (!options.IsDynamicPermissionStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache permissions, so first request doesn't wait
            await DynamicPermissionDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
