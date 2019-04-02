/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging/AuditLoggingPermissions
* 创建者：天上有木月
* 创建时间：2019/4/2 2:18:54
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

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging
{
    public class AuditLoggingPermissions
    {
        public const string GroupName = "AuditLogging";

        public static class AuditLoggs
        {
            public const string Default = GroupName + ".AuditLog";
        }
    }
}
