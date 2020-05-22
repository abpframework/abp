using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class IdentityServerModuleExtensionConfigurationDictionaryExtensions
    {
        public static ModuleExtensionConfigurationDictionary ConfigureIdentityServer(
            this ModuleExtensionConfigurationDictionary modules,
            Action<IdentityServerModuleExtensionConfiguration> configureAction)
        {
            return modules.ConfigureModule(
                IdentityServerModuleExtensionConsts.ModuleName,
                configureAction
            );
        }
    }
}