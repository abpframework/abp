using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

public static class AuditLoggingModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureAuditLogging(
        this ModuleExtensionConfigurationDictionary modules,
        Action<AuditLoggingModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            AuditLoggingModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
