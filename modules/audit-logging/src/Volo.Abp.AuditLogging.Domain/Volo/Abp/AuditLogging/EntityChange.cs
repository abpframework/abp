using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    public class EntityChange : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        public virtual DateTime ChangeTime { get; set; }

        public virtual EntityChangeType ChangeType { get; set; }

        public virtual string EntityId { get; set; }

        public virtual string EntityTypeFullName { get; set; }

        public ICollection<EntityPropertyChangeInfo> PropertyChanges { get; set; }

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
            PropertyChanges = entityChangeInfo.PropertyChanges;
        }
    }
}
