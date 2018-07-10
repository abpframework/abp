using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.AuditLogging
{
    public class AuditLogAction : Entity<Guid>, IHasExtraProperties
    {
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

        public AuditLogAction(Guid id, Guid auditLogId, AuditLogActionInfo auditLogActionInfo)
        {
            Id = id;
            AuditLogId = auditLogId;
            ServiceName = auditLogActionInfo.ServiceName;
            MethodName = auditLogActionInfo.MethodName;
            Parameters = auditLogActionInfo.Parameters;
            ExecutionTime = auditLogActionInfo.ExecutionTime;
            ExecutionDuration = auditLogActionInfo.ExecutionDuration;
            ExtraProperties = auditLogActionInfo.ExtraProperties; //TODO: Copy, instead of assign
        }
    }
}
