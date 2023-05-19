using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public static class OpenIddictModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureOpenIddict(
        this ModuleExtensionConfigurationDictionary modules,
        Action<OpenIddictModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            OpenIddictModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
