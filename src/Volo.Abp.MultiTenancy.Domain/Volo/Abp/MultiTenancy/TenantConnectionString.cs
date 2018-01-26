using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MultiTenancy
{
    public class TenantConnectionString : Entity<Guid> //TODO: PK should be TenantId + Name (so, inherit from Entity)
    {
        public virtual Guid TenantId { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual string Value { get; protected set; }

        protected TenantConnectionString()
        {
            
        }

        public TenantConnectionString(Guid tenantId, [NotNull] string name, [NotNull] string value)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(value, nameof(value));

            TenantId = tenantId;
            Name = name;
            Value = value;
        }
    }
}