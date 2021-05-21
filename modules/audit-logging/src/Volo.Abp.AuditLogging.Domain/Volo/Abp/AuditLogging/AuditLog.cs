using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    [DisableAuditing]
    public class AuditLog : AggregateRoot<Guid>, IMultiTenant
    {
        public virtual string ApplicationName { get; set; }

        public virtual Guid? UserId { get; protected set; }

        public virtual string UserName { get; protected set; }

        public virtual Guid? TenantId { get; protected set; }

        public virtual string TenantName { get; protected set; }

        public virtual Guid? ImpersonatorUserId { get; protected set; }

        public virtual Guid? ImpersonatorTenantId { get; protected set; }

        public virtual DateTime ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }

        public virtual string ClientIpAddress { get; protected set; }

        public virtual string ClientName { get; protected set; }

        public virtual string ClientId { get; set; }

        public virtual string CorrelationId { get; set; }

        public virtual string BrowserInfo { get; protected set; }

        public virtual string HttpMethod { get; protected set; }

        public virtual string Url { get; protected set; }

        public virtual string Exceptions { get; protected set; }

        public virtual string Comments { get; protected set; }

        public virtual int? HttpStatusCode { get; set; }

        public virtual ICollection<EntityChange> EntityChanges { get; protected set; }

        public virtual ICollection<AuditLogAction> Actions { get; protected set; }

        protected AuditLog()
        {

        }

        public AuditLog(
            Guid id,
            string applicationName,
            Guid? tenantId,
            string tenantName,
            Guid? userId,
            string userName,
            DateTime executionTime,
            int executionDuration,
            string clientIpAddress,
            string clientName,
            string clientId,
            string correlationId,
            string browserInfo,
            string httpMethod,
            string url,
            int? httpStatusCode,
            Guid? impersonatorUserId,
            Guid? impersonatorTenantId,
            ExtraPropertyDictionary extraPropertyDictionary,
            List<EntityChange> entityChanges,
            List<AuditLogAction> actions,
            string exceptions,
            string comments)
            : base(id)
        {
            ApplicationName = applicationName.Truncate(AuditLogConsts.MaxApplicationNameLength);
            TenantId = tenantId;
            TenantName = tenantName.Truncate(AuditLogConsts.MaxTenantNameLength);
            UserId = userId;
            UserName = userName.Truncate(AuditLogConsts.MaxUserNameLength);
            ExecutionTime = executionTime;
            ExecutionDuration = executionDuration;
            ClientIpAddress = clientIpAddress.Truncate(AuditLogConsts.MaxClientIpAddressLength);
            ClientName = clientName.Truncate(AuditLogConsts.MaxClientNameLength);
            ClientId = clientId.Truncate(AuditLogConsts.MaxClientIdLength);
            CorrelationId = correlationId.Truncate(AuditLogConsts.MaxCorrelationIdLength);
            BrowserInfo = browserInfo.Truncate(AuditLogConsts.MaxBrowserInfoLength);
            HttpMethod = httpMethod.Truncate(AuditLogConsts.MaxHttpMethodLength);
            Url = url.Truncate(AuditLogConsts.MaxUrlLength);
            HttpStatusCode = httpStatusCode;
            ImpersonatorUserId = impersonatorUserId;
            ImpersonatorTenantId = impersonatorTenantId;

            ExtraProperties = extraPropertyDictionary;
            EntityChanges = entityChanges;
            Actions = actions;
            Exceptions = exceptions.Truncate(AuditLogConsts.MaxExceptionsLengthValue);
            Comments = comments.Truncate(AuditLogConsts.MaxCommentsLength);
        }
    }
}
