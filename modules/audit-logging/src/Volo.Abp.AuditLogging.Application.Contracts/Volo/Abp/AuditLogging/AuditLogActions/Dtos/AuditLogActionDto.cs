/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions.Dtos/AuditLogActionDto
* 创建者：天上有木月
* 创建时间：2019/4/2 2:27:11
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions.Dtos
{
    public class AuditLogActionDto : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }

        public Guid AuditLogId { get; protected set; }

        public string ServiceName { get; protected set; }

        public string MethodName { get; protected set; }

        public string Parameters { get; protected set; }

        public DateTime ExecutionTime { get; protected set; }

        public int ExecutionDuration { get; protected set; }

    }
}
