using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Auditing
{
    public class EntityChangeInfo : IMultiTenant
    {
        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }

        public Guid? TenantId { get; set; }

        public List<EntityPropertyChangeInfo> PropertyChanges { get; set; }

        #region Not mapped

        [NotMapped]
        public virtual object EntityEntry { get; set; } //TODO: ???

        #endregion
    }
}
