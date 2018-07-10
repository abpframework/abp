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
    public class AuditLog : Entity<Guid>, IHasExtraProperties, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        public virtual Guid? UserId { get; set; }

        public virtual Guid? ImpersonatorUserId { get; set; }

        public virtual Guid? ImpersonatorTenantId { get; set; }

        public virtual DateTime ExecutionTime { get; set; }

        public virtual int ExecutionDuration { get; set; }

        public virtual string ClientIpAddress { get; set; }

        public virtual string ClientName { get; set; }

        public virtual string BrowserInfo { get; set; }

        public virtual List<string> Exceptions { get; }

        public Dictionary<string, object> ExtraProperties { get; }

        public ICollection<EntityChange> EntityChanges { get; }

        public ICollection<AuditLogAction> Actions { get; set; }

        protected AuditLog()
        {

        }

        public AuditLog(IGuidGenerator guidGenerator, AuditLogInfo auditInfo)
        {
            Id = guidGenerator.Create();
            TenantId = auditInfo.TenantId;
            UserId = auditInfo.UserId;
            ExecutionTime = auditInfo.ExecutionTime;
            ExecutionDuration = auditInfo.ExecutionDuration;
            ClientIpAddress = auditInfo.ClientIpAddress;
            ClientName = auditInfo.ClientName;
            BrowserInfo = auditInfo.BrowserInfo;
            ImpersonatorUserId = auditInfo.ImpersonatorUserId;
            ImpersonatorTenantId = auditInfo.ImpersonatorTenantId;
            ExtraProperties = auditInfo.ExtraProperties;
            EntityChanges = auditInfo.EntityChanges.Select(e => new EntityChange(e)).ToList();
            Actions = auditInfo.Actions.Select(e => new AuditLogAction(guidGenerator.Create(), Id, e)).ToList();
            Exceptions = auditInfo.Exceptions.Select(e => e.ToString()).ToList();
        }
    }
}
