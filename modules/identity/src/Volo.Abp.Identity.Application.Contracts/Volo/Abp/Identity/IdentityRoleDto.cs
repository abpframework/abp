using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityRoleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}