using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class IdentityModuleObjectExtensionConfigurationsExtensions
    {
        public static ModuleObjectExtensionConfigurationDictionary ConfigureIdentity(
            this ModuleObjectExtensionConfigurationDictionary modules,
            Action<IdentityModuleObjectExtensionConfiguration> configureAction)
        {
            return modules.ConfigureModule(
                "Identity",
                configureAction
            );
        }
    }
}
