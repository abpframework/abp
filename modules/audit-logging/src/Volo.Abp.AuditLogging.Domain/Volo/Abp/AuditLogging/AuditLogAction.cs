using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    [DisableAuditing]
    public class AuditLogAction : Entity<Guid>, IMultiTenant, IHasExtraProperties
    {
        public Guid? TenantId { get; protected set; }

        public virtual Guid AuditLogId { get; protected set; }

        public virtual string ServiceName { get; protected set; }

        public virtual string MethodName { get; protected set; }

        public virtual string Parameters { get; protected set; }

        public virtual DateTime ExecutionTime { get; protected set; }

        public virtual int ExecutionDuration { get; protected set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }
        
        protected AuditLogAction()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public AuditLogAction(Guid id, Guid auditLogId, AuditLogActionInfo actionInfo)
        {
            Id = id;
            TenantId = actionInfo.TenantId;
            AuditLogId = auditLogId;
            ServiceName = actionInfo.ServiceName;
            MethodName = actionInfo.MethodName;
            Parameters = actionInfo.Parameters;
            ExecutionTime = actionInfo.ExecutionTime;
            ExecutionDuration = actionInfo.ExecutionDuration;
            ExtraProperties = actionInfo.ExtraProperties.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
