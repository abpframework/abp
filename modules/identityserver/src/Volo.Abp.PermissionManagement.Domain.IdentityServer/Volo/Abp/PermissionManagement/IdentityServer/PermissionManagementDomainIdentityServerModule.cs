using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.IdentityServer
{
    [DependsOn(
        typeof(IdentityServerDomainSharedModule),
        typeof(PermissionManagementDomainModule)
    )]
    public class PermissionManagementDomainIdentityServerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<ClientPermissionManagementProvider>();

                options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "IdentityServer.Client.ManagePermissions";
            });
        }
    }
}
