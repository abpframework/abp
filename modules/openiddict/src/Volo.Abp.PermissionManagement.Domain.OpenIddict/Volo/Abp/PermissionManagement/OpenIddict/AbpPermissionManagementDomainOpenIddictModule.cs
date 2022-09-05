using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;

namespace Volo.Abp.PermissionManagement.OpenIddict;

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
            options.ManagementProviders.Add<ApplicationPermissionManagementProvider>();
            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "OpenIddictPro.Application.ManagePermissions";
        });
    }
}
