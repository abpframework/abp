using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Auditing
{
    public class EntityChangeInfo : IMultiTenant, IHasExtraProperties
    {
        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }

        public Guid? TenantId { get; set; }

        public List<EntityPropertyChangeInfo> PropertyChanges { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public virtual object EntityEntry { get; set; } //TODO: Try to remove since it breaks serializability

        public EntityChangeInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}
