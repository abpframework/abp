using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.Abp.Identity
{
    public class ChangePasswordInput
    {
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string CurrentPassword { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string NewPassword { get; set; }
    }
}
