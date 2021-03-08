using System;

namespace Volo.Abp.Identity
{
    [Serializable]
    public class IdentityRoleNameChangedEto
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public string Name { get; set; }

        public string OldName { get; set; }
    }
}