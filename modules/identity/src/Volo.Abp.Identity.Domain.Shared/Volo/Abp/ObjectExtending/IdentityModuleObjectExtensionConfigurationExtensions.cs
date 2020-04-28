using System;

namespace Volo.Abp.ObjectExtending
{
    public static class IdentityModuleObjectExtensionConfigurationExtensions
    {
        public static IdentityModuleObjectExtensionConfiguration ConfigureUser(
            this IdentityModuleObjectExtensionConfiguration configurations,
            Action<ModuleEntityObjectExtensionConfiguration> configureAction)
        {
            return configurations.ConfigureEntity(
                "User",
                configureAction
            );
        }
    }
}