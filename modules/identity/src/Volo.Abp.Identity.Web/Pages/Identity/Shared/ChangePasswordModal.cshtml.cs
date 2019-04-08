using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity.Localization;

namespace Volo.Abp.Identity.Web.Pages.Identity.Shared
{
    public class ChangePasswordModal : AbpPageModel
    {
        [BindProperty]
        public ChangePasswordInfoModel ChangePasswordInfoModel { get; set; }

        private readonly IProfileAppService _profileAppService;
        private readonly IStringLocalizer<IdentityResource> _localizer;

        public ChangePasswordModal(
            IProfileAppService profileAppService,
            IStringLocalizer<IdentityResource> localizer)
        {
            _profileAppService = profileAppService;
            _localizer = localizer;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            if (ChangePasswordInfoModel.NewPassword != ChangePasswordInfoModel.NewPasswordConfirm)
            {
                throw new UserFriendlyException(_localizer.GetString("Identity.PasswordConfirmationFailed").Value);
            }

            await _profileAppService.ChangePasswordAsync(
                ChangePasswordInfoModel.CurrentPassword,
                ChangePasswordInfoModel.NewPassword
            );

            return NoContent();
        }
    }

    public class ChangePasswordInfoModel
    {
        [Required]
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        [Display(Name = "DisplayName:CurrentPassword")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        [Display(Name = "DisplayName:NewPassword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        [Display(Name = "DisplayName:NewPasswordConfirm")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}