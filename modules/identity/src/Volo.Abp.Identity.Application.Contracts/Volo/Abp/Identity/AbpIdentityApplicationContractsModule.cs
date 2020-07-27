using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainSharedModule),
        typeof(AbpUsersAbstractionModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpPermissionManagementApplicationContractsModule)
        )]
    public class AbpIdentityApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.Role,
                    getApiTypes: new[] { typeof(IdentityRoleDto) },
                    createApiTypes: new[] { typeof(IdentityRoleCreateDto) },
                    updateApiTypes: new[] { typeof(IdentityRoleUpdateDto) }
                );
            
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    getApiTypes: new[] { typeof(IdentityUserDto) },
                    createApiTypes: new[] { typeof(IdentityUserCreateDto) },
                    updateApiTypes: new[] { typeof(IdentityUserUpdateDto) }
                );
        }
    }
}