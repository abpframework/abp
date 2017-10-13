using System;

namespace Volo.Abp.Identity
{
    public class IdentityUserRoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}
