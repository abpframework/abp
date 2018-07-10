using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AuditLogging
{
    public class EntityPropertyChange : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }

        public virtual Guid EntityChangeId { get; protected set; }

        public virtual string NewValue { get; protected set; }

        public virtual string OriginalValue { get; protected set; }

        public virtual string PropertyName { get; protected set; }

        public virtual string PropertyTypeFullName { get; protected set; }

        protected EntityPropertyChange()
        {

        }

        public EntityPropertyChange(IGuidGenerator guidGenerator, Guid entityChangeId, EntityPropertyChangeInfo entityChangeInfo)
        {
            Id = guidGenerator.Create();
            TenantId = entityChangeInfo.TenantId;
            EntityChangeId = entityChangeId;
            NewValue = entityChangeInfo.NewValue;
            OriginalValue = entityChangeInfo.OriginalValue;
            PropertyName = entityChangeInfo.PropertyName;
            PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName;
        }
    }
}