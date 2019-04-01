using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Auditing
{
    [Serializable]
    public class AuditLogActionInfo : IMultiTenant, IHasExtraProperties
    {
        public Guid? TenantId { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public AuditLogActionInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}