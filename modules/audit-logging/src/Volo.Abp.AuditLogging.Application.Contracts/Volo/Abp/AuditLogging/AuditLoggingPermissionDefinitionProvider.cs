using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging
{
    public class AuditLoggingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var loggingGroup = context.AddGroup(AuditLoggingPermissions.GroupName, L("Permission:AuditLogging"));

            var logs = loggingGroup.AddPermission(AuditLoggingPermissions.AuditLoggs.Default, L("Permission:AuditLogging:AuditLog"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuditLoggingResource>(name);
        }
    }
}
