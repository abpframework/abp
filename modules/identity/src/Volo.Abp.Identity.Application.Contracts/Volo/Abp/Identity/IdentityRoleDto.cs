using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityRoleDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }

        public bool IsDefault { get; set; }
        
        public bool IsStatic { get; set; }

        public bool IsPublic { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}