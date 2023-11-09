using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public static class CmsKitModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureCmsKit(
        this ModuleExtensionConfigurationDictionary modules,
        Action<CmsKitModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            CmsKitModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}