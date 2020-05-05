using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.AuditLogging
{
    [DependsOn(typeof(AbpAuditingModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpAuditLoggingDomainSharedModule))]
    public class AbpAuditLoggingDomainModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                AuditLoggingModuleExtensionConsts.ModuleName,
                AuditLoggingModuleExtensionConsts.EntityNames.AuditLog,
                typeof(AuditLog)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                AuditLoggingModuleExtensionConsts.ModuleName,
                AuditLoggingModuleExtensionConsts.EntityNames.AuditLogAction,
                typeof(AuditLogAction)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                AuditLoggingModuleExtensionConsts.ModuleName,
                AuditLoggingModuleExtensionConsts.EntityNames.EntityChange,
                typeof(EntityChange)
            );
        }
    }
}
