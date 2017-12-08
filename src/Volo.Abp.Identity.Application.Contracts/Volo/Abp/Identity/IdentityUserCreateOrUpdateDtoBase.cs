using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Volo.Abp.Identity
{
    public abstract class IdentityUserCreateOrUpdateDtoBase
    {
        [Required]
        [MaxLength(IdentityUserConsts.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(IdentityUserConsts.MaxEmailLength)]
        public string Email { get; set; }

        [MaxLength(IdentityUserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; } //TODO: Optional?

        public bool LockoutEnabled { get; set; } //TODO: Optional?

        [CanBeNull]
        public string[] RoleNames { get; set; }
    }
}