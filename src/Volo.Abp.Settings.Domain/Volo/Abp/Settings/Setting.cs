using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Settings
{
    public class Setting : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }

        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string Value { get; internal set; }

        [CanBeNull]
        public virtual string EntityType { get; protected set; }

        [CanBeNull]
        public virtual string EntityId { get; protected set; }

        protected Setting()
        {

        }

        public Setting(
            Guid id, 
            [NotNull] string name, 
            [NotNull] string value, 
            [CanBeNull] string entityType = null, 
            [CanBeNull] string entityId = null, 
            Guid? tenantId = null)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(value, nameof(value));

            Id = id;
            Name = name;
            Value = value;
            EntityType = entityType;
            EntityId = entityId;
            TenantId = tenantId;
        }
    }
}