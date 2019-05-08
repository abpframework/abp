using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
    {
        [Required]
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        public string Password { get; set; }

        public bool SendActivationEmail { get; set; } = false;
    }
}