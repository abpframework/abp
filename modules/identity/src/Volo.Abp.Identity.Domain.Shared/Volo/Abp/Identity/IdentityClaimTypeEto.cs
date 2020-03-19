using System;

namespace Volo.Abp.Identity
{
    [Serializable]
    public class IdentityClaimTypeEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool IsStatic { get; set; }

        public virtual string Regex { get; set; }

        public virtual string RegexDescription { get; set; }

        public virtual string Description { get; set; }

        public virtual IdentityClaimValueType ValueType { get; set; }
    }
}
