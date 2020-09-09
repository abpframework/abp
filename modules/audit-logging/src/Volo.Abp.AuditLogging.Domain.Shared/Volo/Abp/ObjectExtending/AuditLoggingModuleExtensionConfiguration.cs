using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending
{
    public class AuditLoggingModuleExtensionConfiguration : ModuleExtensionConfiguration
    {
        public AuditLoggingModuleExtensionConfiguration ConfigureAuditLog(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                AuditLoggingModuleExtensionConsts.EntityNames.AuditLog,
                configureAction
            );
        }

        public AuditLoggingModuleExtensionConfiguration ConfigureAuditLogAction(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                AuditLoggingModuleExtensionConsts.EntityNames.AuditLogAction,
                configureAction
            );
        }

        public AuditLoggingModuleExtensionConfiguration ConfigureEntityChange(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                AuditLoggingModuleExtensionConsts.EntityNames.EntityChange,
                configureAction
            );
        }
    }
}
