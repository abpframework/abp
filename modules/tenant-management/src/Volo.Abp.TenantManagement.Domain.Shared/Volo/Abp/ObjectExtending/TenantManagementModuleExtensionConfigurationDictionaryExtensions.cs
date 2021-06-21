using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public static class TenantManagementModuleExtensionConfigurationDictionaryExtensions
    {
        public static ModuleExtensionConfigurationDictionary ConfigureTenantManagement(
            this ModuleExtensionConfigurationDictionary modules,
            Action<TenantManagementModuleExtensionConfiguration> configureAction)
        {
            return modules.ConfigureModule(
                TenantManagementModuleExtensionConsts.ModuleName,
                configureAction
            );
        }
    }
}