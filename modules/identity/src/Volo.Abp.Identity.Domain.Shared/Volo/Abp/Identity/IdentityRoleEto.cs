using System;

namespace Volo.Abp.Identity
{
    [Serializable]
    public class IdentityRoleEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string NormalizedName { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual bool IsStatic { get; set; }

        public virtual bool IsPublic { get; set; }
    }
}