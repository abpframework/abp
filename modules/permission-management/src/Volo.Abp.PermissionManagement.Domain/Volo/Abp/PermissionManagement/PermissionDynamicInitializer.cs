using System;
using System.Threading;
using System.Threading.Tasks;
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

    public PermissionDynamicInitializer(IServiceProvider serviceProvider)
    {
        Logger = NullLogger<PermissionDynamicInitializer>.Instance;

        ServiceProvider = serviceProvider;
    }

    public virtual Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
    {
        var options = ServiceProvider
            .GetRequiredService<IOptions<PermissionManagementOptions>>()
            .Value;

        if (!options.SaveStaticPermissionsToDatabase && !options.IsDynamicPermissionStoreEnabled)
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
        PermissionManagementOptions options,
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

                await SaveStaticPermissionsToDatabaseAsync(options, cancellationToken);

                if (cancellationTokenProvider.Token.IsCancellationRequested)
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

        var staticPermissionSaver = ServiceProvider.GetRequiredService<IStaticPermissionSaver>();

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
                    await staticPermissionSaver.SaveAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationToken);
    }

    protected virtual async Task PreCacheDynamicPermissionsAsync(
        PermissionManagementOptions options)
    {
        if (!options.IsDynamicPermissionStoreEnabled)
        {
            return;
        }

        var dynamicPermissionDefinitionStore = ServiceProvider.GetRequiredService<IDynamicPermissionDefinitionStore>();

        try
        {
            // Pre-cache permissions, so first request doesn't wait
            await dynamicPermissionDefinitionStore.GetGroupsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);

            throw; // It will be cached in Initialize()
        }
    }
}
