using Volo.Abp.Authorization.Permissions;
using Volo.Abp.OpenIddict;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement.OpenIddict
{
    [DependsOn(
        typeof(AbpOpenIddictDomainSharedModule),
        typeof(AbpPermissionManagementDomainModule)
    )]
    public class AbpPermissionManagementDomainOpenIddictModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<ClientPermissionManagementProvider>();

                options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "OpenIddict.Client.ManagePermissions";
            });
        }
    }
}
