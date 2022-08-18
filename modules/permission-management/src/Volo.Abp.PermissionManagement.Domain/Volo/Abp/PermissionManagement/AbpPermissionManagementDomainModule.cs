using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement;

[DependsOn(typeof(AbpAuthorizationModule))]
[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpPermissionManagementDomainSharedModule))]
[DependsOn(typeof(AbpCachingModule))]
[DependsOn(typeof(AbpJsonModule))]
public class AbpPermissionManagementDomainModule : AbpModule
{
    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        if (context
            .ServiceProvider
            .GetRequiredService<IOptions<PermissionManagementOptions>>()
            .Value
            .SaveStaticPermissionsToDatabase)
        {
            SaveStaticPermissionsToDatabase(context);
        }
        
        return Task.CompletedTask;
    }

    private static void SaveStaticPermissionsToDatabase(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();

            try
            {
                await Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10))
                    .ExecuteAsync(async () =>
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
                    });
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }
}