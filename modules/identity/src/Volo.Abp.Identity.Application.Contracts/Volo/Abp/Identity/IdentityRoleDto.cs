using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityRoleDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public bool IsStatic { get; set; }

        public bool IsPublic { get; set; }
    }
}