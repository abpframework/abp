using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    //TODO: Use different Dtos for Create & Update even if they are derived from a base Dto. Thus, clients will be backward compatible in a future change.
    public class IdentityUserCreateOrUpdateDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; } //TODO: Optional?

        public bool LockoutEnabled { get; set; } //TODO: Optional?

        [Required]
        [MaxLength(16)] //TODO: Create a shared dll of Identity and move consts to there for sharing!
        public string Password { get; set; }
    }
}