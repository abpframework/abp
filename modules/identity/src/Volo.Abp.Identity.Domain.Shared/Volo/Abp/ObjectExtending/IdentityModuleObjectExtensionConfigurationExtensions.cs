using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class IdentityModuleObjectExtensionConfigurationExtensions
    {
        public static IdentityModuleObjectExtensionConfiguration ConfigureUser(
            this IdentityModuleObjectExtensionConfiguration configurations,
            Action<ModuleEntityObjectExtensionConfiguration> configureAction)
        {
            return configurations.ConfigureObject(
                "User",
                configureAction
            );
        }
    }
}