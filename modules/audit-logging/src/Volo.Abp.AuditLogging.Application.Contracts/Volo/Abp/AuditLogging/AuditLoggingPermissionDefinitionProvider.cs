/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging/AuditLoggingPermissionDefinitionProvider
* 创建者：天上有木月
* 创建时间：2019/4/2 2:18:08
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
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
            var bloggingGroup = context.AddGroup(AuditLoggingPermissions.GroupName, L("Permission:AuditLogging"));

            var blogs = bloggingGroup.AddPermission(AuditLoggingPermissions.AuditLoggs.Default, L("Permission:AuditLogging"));
        }


        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuditLoggingResource>(name);
        }
    }
}
