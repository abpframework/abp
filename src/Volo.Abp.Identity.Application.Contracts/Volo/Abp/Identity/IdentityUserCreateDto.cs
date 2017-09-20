using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
    {
        [Required]
        [MaxLength(IdentityUserConsts.MaxPasswordLength)]
        public string Password { get; set; }
    }
}