using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Password
{
    [Widget(
        ScriptTypes = new[] { typeof(ProfileManagementScriptBundleContributor) }
    )]
    public class AccountProfilePasswordManagementGroupViewComponent : AbpViewComponent
    {
        private readonly IProfileAppService _profileAppService;

        public AccountProfilePasswordManagementGroupViewComponent(
            IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _profileAppService.GetAsync();

            var model = new ChangePasswordInfoModel
            {
                HideOldPasswordInput = !user.HasPassword
            };

            return View("~/Pages/Account/Components/ProfileManagementGroup/Password/Default.cshtml", model);
        }

        public class ChangePasswordInfoModel
        {
            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [Display(Name = "DisplayName:CurrentPassword")]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string CurrentPassword { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [Display(Name = "DisplayName:NewPassword")]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string NewPassword { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [Display(Name = "DisplayName:NewPasswordConfirm")]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string NewPasswordConfirm { get; set; }

            public bool HideOldPasswordInput { get; set; }
        }
    }
}
