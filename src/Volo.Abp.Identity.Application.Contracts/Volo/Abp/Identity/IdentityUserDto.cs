using Volo.Abp.Application.Services.Dtos;

namespace Volo.Abp.Identity
{
    public class IdentityUserDto : EntityDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}