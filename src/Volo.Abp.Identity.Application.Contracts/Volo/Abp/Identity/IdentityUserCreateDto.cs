using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityUserCreateDto: IdentityUserCreateOrUpdateDtoBase
    {
        [Required]
        [MaxLength(16)] //TODO: Create a shared dll of Identity and move consts to there for sharing!
        public string Password { get; set; }
    }
}