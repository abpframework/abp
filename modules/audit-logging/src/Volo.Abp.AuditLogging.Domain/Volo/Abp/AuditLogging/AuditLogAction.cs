using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    public class AuditLogAction : Entity<Guid>
    {
        public virtual string ServiceName { get; set; }

        public virtual string MethodName { get; set; }

        public virtual string Parameters { get; set; }

        public virtual DateTime ExecutionTime { get; set; }

        public virtual int ExecutionDuration { get; set; }

        public virtual Dictionary<string, object> ExtraProperties { get; }

        protected AuditLogAction()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public AuditLogAction(AuditLogActionInfo auditLogActionInfo)
        {
            ServiceName = auditLogActionInfo.ServiceName;
            MethodName = auditLogActionInfo.MethodName;
            Parameters = auditLogActionInfo.Parameters;
            ExecutionTime = auditLogActionInfo.ExecutionTime;
            ExecutionDuration = auditLogActionInfo.ExecutionDuration;
            ExtraProperties = auditLogActionInfo.ExtraProperties;
        }
    }
}
