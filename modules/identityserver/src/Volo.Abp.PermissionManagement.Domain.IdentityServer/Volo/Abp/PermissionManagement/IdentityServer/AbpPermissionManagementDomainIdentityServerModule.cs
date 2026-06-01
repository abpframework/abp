using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.IdentityServer;

[DependsOn(
    typeof(AbpIdentityServerDomainSharedModule),
    typeof(AbpPermissionManagementDomainModule)
)]
public class AbpPermissionManagementDomainIdentityServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<ClientPermissionManagementProvider>();

            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "IdentityServer.Client.ManagePermissions";
        });

        context.Services.AddAbpOptions<PermissionManagementOptions>().PostConfigure<IServiceProvider>((options, serviceProvider) =>
        {
            // The IClientFinder implementation in identity Server Pro module for tiered application.
            if (serviceProvider.GetService<IClientFinder>() == null)
            {
                return;
            }

            options.ResourceManagementProviders.Add<ClientResourcePermissionManagementProvider>();
            options.ResourcePermissionProviderKeyLookupServices.Add<ClientResourcePermissionProviderKeyLookupService>();
        });
    }
}
