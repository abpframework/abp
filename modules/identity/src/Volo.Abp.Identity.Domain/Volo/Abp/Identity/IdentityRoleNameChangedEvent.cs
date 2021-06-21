using System;

namespace Volo.Abp.Identity
{
    [Obsolete("Use the distributed event (IdentityRoleNameChangedEto) instead.")]
    public class IdentityRoleNameChangedEvent
    {
        public IdentityRole IdentityRole { get; set; }
        public string OldName { get; set; }
    }
}
