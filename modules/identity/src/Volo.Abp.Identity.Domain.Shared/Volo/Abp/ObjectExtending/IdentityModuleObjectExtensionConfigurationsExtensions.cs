using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class IdentityModuleObjectExtensionConfigurationsExtensions
    {
        public static ModuleExtensionConfigurationDictionary ConfigureIdentity(
            this ModuleExtensionConfigurationDictionary modules,
            Action<IdentityModuleExtensionConfiguration> configureAction)
        {
            return modules.ConfigureModule(
                "Identity",
                configureAction
            );
        }
    }
}
