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
    public class EntityChange : Entity<Guid>, IMultiTenant, IHasExtraProperties
    {
        public virtual Guid AuditLogId { get; protected set; }

        public virtual Guid? TenantId { get; protected set; }

        public virtual DateTime ChangeTime { get; protected set; }

        public virtual EntityChangeType ChangeType { get; protected set; }

        public virtual Guid? EntityTenantId { get; protected set; }

        public virtual string EntityId { get; protected set; }

        public virtual string EntityTypeFullName { get; protected set; }

        public virtual ICollection<EntityPropertyChange> PropertyChanges { get; protected set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        protected EntityChange()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public EntityChange(
            IGuidGenerator guidGenerator, 
            Guid auditLogId, 
            EntityChangeInfo entityChangeInfo,
            Guid? tenantId = null)
        {
            Id = guidGenerator.Create();
            AuditLogId = auditLogId;
            TenantId = tenantId;
            ChangeTime = entityChangeInfo.ChangeTime;
            ChangeType = entityChangeInfo.ChangeType;
            EntityId = entityChangeInfo.EntityId.Truncate(EntityChangeConsts.MaxEntityTypeFullNameLength);
            EntityTypeFullName = entityChangeInfo.EntityTypeFullName.TruncateFromBeginning(EntityChangeConsts.MaxEntityTypeFullNameLength);

            PropertyChanges = entityChangeInfo
                                  .PropertyChanges?
                                  .Select(p => new EntityPropertyChange(guidGenerator, Id, p, tenantId))
                                  .ToList()
                              ?? new List<EntityPropertyChange>();

            ExtraProperties = entityChangeInfo
                                  .ExtraProperties?
                                  .ToDictionary(pair => pair.Key, pair => pair.Value)
                              ?? new Dictionary<string, object>();
        }
    }
}
