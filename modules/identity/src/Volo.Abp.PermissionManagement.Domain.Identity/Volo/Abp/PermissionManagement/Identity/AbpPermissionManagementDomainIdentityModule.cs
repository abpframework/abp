using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Volo.Abp.PermissionManagement.Identity;

[DependsOn(
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpPermissionManagementDomainModule),
    typeof(AbpUsersAbstractionModule)
)]
public class AbpPermissionManagementDomainIdentityModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<UserPermissionManagementProvider>();
            options.ManagementProviders.Add<RolePermissionManagementProvider>();

            //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
            options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "AbpIdentity.Users.ManagePermissions";
            options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "AbpIdentity.Roles.ManagePermissions";

            options.ResourceManagementProviders.Add<UserResourcePermissionManagementProvider>();
            options.ResourceManagementProviders.Add<RoleResourcePermissionManagementProvider>();

            options.ResourcePermissionProviderKeyLookupServices.Add<UserResourcePermissionProviderKeyLookupService>();
            options.ResourcePermissionProviderKeyLookupServices.Add<RoleResourcePermissionProviderKeyLookupService>();
        });
    }
}
