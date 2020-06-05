using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class ManageModel : AccountPageModel
    {
        public ChangePasswordInfoModel ChangePasswordInfoModel { get; set; }

        public PersonalSettingsInfoModel PersonalSettingsInfoModel { get; set; }

        protected IProfileAppService ProfileAppService { get; }

        public ManageModel(IProfileAppService profileAppService)
        {
            ProfileAppService = profileAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var user = await ProfileAppService.GetAsync();

            PersonalSettingsInfoModel = ObjectMapper.Map<ProfileDto, PersonalSettingsInfoModel>(user);

            return Page();
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());
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
    public class PersonalSettingsInfoModel
    {
        [Required]
        [StringLength(IdentityUserConsts.MaxUserNameLength)]
        [Display(Name = "DisplayName:UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(IdentityUserConsts.MaxEmailLength)]
        [Display(Name = "DisplayName:Email")]
        public string Email { get; set; }

        [StringLength(IdentityUserConsts.MaxNameLength)]
        [Display(Name = "DisplayName:Name")]
        public string Name { get; set; }

        [StringLength(IdentityUserConsts.MaxSurnameLength)]
        [Display(Name = "DisplayName:Surname")]
        public string Surname { get; set; }

        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        [Display(Name = "DisplayName:PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
