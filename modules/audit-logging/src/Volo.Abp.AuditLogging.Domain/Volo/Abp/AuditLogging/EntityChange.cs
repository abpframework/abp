using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    public class EntityChange : Entity<Guid>, IMultiTenant
    {
        public virtual Guid AuditLogId { get; protected set; }

        public virtual Guid? TenantId { get; protected set; }

        public virtual DateTime ChangeTime { get; protected set; }

        public virtual EntityChangeType ChangeType { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual string EntityTypeFullName { get; protected set; }

        public ICollection<EntityPropertyChangeInfo> PropertyChanges { get; protected set; }

        protected EntityChange()
        {

        }

        public EntityChange(EntityChangeInfo entityChangeInfo)
        {
            TenantId = entityChangeInfo.TenantId;
            ChangeTime = entityChangeInfo.ChangeTime;
            ChangeType = entityChangeInfo.ChangeType;
            EntityId = entityChangeInfo.EntityId;
            EntityTypeFullName = entityChangeInfo.EntityTypeFullName;
            PropertyChanges = entityChangeInfo.PropertyChanges; //Copy instead of assiging
        }
    }
}
