using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityUserCreateOrUpdateOutput
    {
        public IdentityUserDto User { get; set; }

        public IdentityUserRoleDto[] Roles { get; set; }
    }
}