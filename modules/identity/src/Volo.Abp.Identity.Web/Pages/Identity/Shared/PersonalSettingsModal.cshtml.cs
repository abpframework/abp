using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity.Localization;

namespace Volo.Abp.Identity.Web.Pages.Identity.Shared
{
    public class PersonalSettingsModal : AbpPageModel
    {
        [BindProperty]
        public PersonalSettingsInfoModel PersonalSettingsInfoModel { get; set; }

        private readonly IProfileAppService _profileAppService;

        public PersonalSettingsModal(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        public async Task OnGetAsync()
        {
            var user = await _profileAppService.GetAsync();

            PersonalSettingsInfoModel = ObjectMapper.Map<ProfileDto, PersonalSettingsInfoModel>(user);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var updateDto = ObjectMapper.Map<PersonalSettingsInfoModel, UpdateProfileDto>(PersonalSettingsInfoModel);

            await _profileAppService.UpdateAsync(updateDto);

            return NoContent();
        }
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