using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        if (context
            .ServiceProvider
            .GetRequiredService<IOptions<PermissionManagementOptions>>()
            .Value
            .SaveStaticPermissionsToDatabase)
        {
            SaveStaticPermissionsToDatabase(context);
        }
    }

    private static void SaveStaticPermissionsToDatabase(ApplicationInitializationContext context)
    {
        Task.Run(async () =>
        {
            using var scope = context
                .ServiceProvider
                .GetRequiredService<IRootServiceProvider>()
                .CreateScope();

            try
            {
                await scope
                    .ServiceProvider
                    .GetRequiredService<IStaticPermissionSaver>()
                    .SaveAsync();
            }
            catch (Exception ex)
            {
                //TODO: We should retry until it's successful!

                scope.ServiceProvider
                    .GetService<ILogger<AbpPermissionManagementDomainModule>>()
                    .LogException(ex);
            }
        });
    }
}