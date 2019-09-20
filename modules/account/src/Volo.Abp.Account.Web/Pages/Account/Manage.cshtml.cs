using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class ManageModel : AccountPageModel
    {
        public ChangePasswordInfoModel ChangePasswordInfoModel { get; set; }

        public PersonalSettingsInfoModel PersonalSettingsInfoModel { get; set; }

        private readonly IProfileAppService _profileAppService;

        public ManageModel(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        public async Task OnGetAsync()
        {
            var user = await _profileAppService.GetAsync();

            PersonalSettingsInfoModel = ObjectMapper.Map<ProfileDto, PersonalSettingsInfoModel>(user);
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
