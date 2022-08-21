using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.PermissionManagement;

[DependsOn(typeof(AbpAuthorizationModule))]
[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpPermissionManagementDomainSharedModule))]
[DependsOn(typeof(AbpCachingModule))]
[DependsOn(typeof(AbpJsonModule))]
public class AbpPermissionManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeDynamicPermissions(context);
        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void InitializeDynamicPermissions(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<PermissionManagementOptions>>()
            .Value;
        
        if (!options.SaveStaticPermissionsToDatabase && !options.IsDynamicPermissionStoreEnabled)
        {
            return;
        }
        
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        Task.Run(async () =>
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
                    
                    await SaveStaticPermissionsToDatabaseAsync(options, scope, cancellationTokenProvider);

                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await PreCacheDynamicPermissionsAsync(options, scope);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }

    private async static Task SaveStaticPermissionsToDatabaseAsync(
        PermissionManagementOptions options,
        IServiceScope scope,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticPermissionsToDatabase)
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
                        .GetRequiredService<IStaticPermissionSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    scope.ServiceProvider
                        .GetService<ILogger<AbpPermissionManagementDomainModule>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationTokenProvider.Token);
    }

    private async static Task PreCacheDynamicPermissionsAsync(PermissionManagementOptions options, IServiceScope scope)
    {
        if (!options.IsDynamicPermissionStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache permissions, so first request doesn't wait 
            await scope
                .ServiceProvider
                .GetRequiredService<IDynamicPermissionDefinitionStore>()
                .GetGroupsAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            scope
                .ServiceProvider
                .GetService<ILogger<AbpPermissionManagementDomainModule>>()?
                .LogException(ex);

            throw; // It will be cached in InitializeDynamicPermissions
        }
    }
}