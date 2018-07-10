using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace Volo.Abp.AuditLogging
{
    public class EntityPropertyChange : Entity<Guid>
    {
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
            EntityChangeId = entityChangeId;
            Id = guidGenerator.Create();
            NewValue = entityChangeInfo.NewValue;
            OriginalValue = entityChangeInfo.OriginalValue;
            PropertyName = entityChangeInfo.PropertyName;
            PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName;
        }
    }
}