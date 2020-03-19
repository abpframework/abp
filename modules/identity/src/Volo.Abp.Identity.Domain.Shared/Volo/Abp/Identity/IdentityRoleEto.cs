using System;

namespace Volo.Abp.Identity
{
    public class IdentityRoleEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public virtual string Name { get; protected internal set; }

        public virtual string NormalizedName { get; protected internal set; }

        public virtual bool IsDefault { get; set; }

        public virtual bool IsStatic { get; set; }

        public virtual bool IsPublic { get; set; }
    }
}