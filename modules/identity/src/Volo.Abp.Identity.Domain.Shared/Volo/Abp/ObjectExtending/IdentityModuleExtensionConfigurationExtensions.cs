using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class IdentityModuleExtensionConfigurationExtensions
    {
        public static IdentityModuleExtensionConfiguration ConfigureUser(
            this IdentityModuleExtensionConfiguration configurations,
            Action<EntityExtensionConfiguration> configureAction)
        {
            return configurations.ConfigureEntity(
                IdentityModuleExtensionConsts.EntityNames.User,
                configureAction
            );
        }
    }
}