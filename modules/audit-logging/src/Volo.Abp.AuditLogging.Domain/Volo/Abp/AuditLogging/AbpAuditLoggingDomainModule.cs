using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.AuditLogging;

[DependsOn(typeof(AbpAuditingModule))]
[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpAuditLoggingDomainSharedModule))]
[DependsOn(typeof(AbpExceptionHandlingModule))]
[DependsOn(typeof(AbpJsonModule))]
public class AbpAuditLoggingDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
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
        });
    }
}
