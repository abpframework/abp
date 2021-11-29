using System;

namespace Volo.Abp.Identity
{
    [Serializable]
    public class IdentityRoleEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public string Name { get; set; }
        
        public bool IsDefault { get; set; }

        public bool IsStatic { get; set; }
        
        public bool IsPublic { get; set; }
    }
}