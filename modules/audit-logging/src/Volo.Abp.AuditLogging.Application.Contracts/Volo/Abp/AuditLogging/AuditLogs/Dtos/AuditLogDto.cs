/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos/AuditLogDto
* 创建者：天上有木月
* 创建时间：2019/4/2 2:24:23
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos
{
    public class AuditLogDto : AggregateRoot<Guid>, IMultiTenant
    {
        public  string ApplicationName { get; set; }

        public  Guid? UserId { get; protected set; }

        public  string UserName { get; protected set; }

        public  Guid? TenantId { get; protected set; }

        public  string TenantName { get; protected set; }

        public  Guid? ImpersonatorUserId { get; protected set; }

        public  Guid? ImpersonatorTenantId { get; protected set; }

        public  DateTime ExecutionTime { get; protected set; }

        public  int ExecutionDuration { get; protected set; }

        public  string ClientIpAddress { get; protected set; }

        public  string ClientName { get; protected set; }

        public  string ClientId { get; set; }

        public  string CorrelationId { get; set; }

        public  string BrowserInfo { get; protected set; }

        public  string HttpMethod { get; protected set; }

        public  string Url { get; protected set; }

        public  string Exceptions { get; protected set; }

        public  string Comments { get; protected set; }

        public  int? HttpStatusCode { get; set; }
    }
}
