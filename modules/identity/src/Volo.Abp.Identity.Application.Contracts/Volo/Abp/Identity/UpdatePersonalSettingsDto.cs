using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class UpdatePersonalSettingsDto
    {
        [StringLength(IdentityUserConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(IdentityUserConsts.MaxSurnameLength)]
        public string Surname { get; set; }

        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
    }
}