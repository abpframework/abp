using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityUserDto : EntityDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}