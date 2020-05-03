using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public class IdentityModuleExtensionConfiguration : ModuleExtensionConfiguration
    {
        public IdentityModuleExtensionConfiguration ConfigureUser(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                IdentityModuleExtensionConsts.EntityNames.User,
                configureAction
            );
        }

        public IdentityModuleExtensionConfiguration ConfigureRole(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                IdentityModuleExtensionConsts.EntityNames.Role,
                configureAction
            );
        }
    }
}