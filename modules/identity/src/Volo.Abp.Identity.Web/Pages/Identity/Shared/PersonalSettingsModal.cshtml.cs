using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity.Web.Pages.Identity.Shared
{
    public class PersonalSettingsModal : AbpPageModel
    {
        [BindProperty]
        public PersonalSettingsInfoModel PersonalSettingsInfoModel { get; set; }

        [BindProperty]
        public PersonalSettingsInfoModel PersonalSettingsInfoModel2 { get; set; }
       
        public bool IsUsernameUpdateDisabled => !string.Equals(
            SettingManager.GetOrNull(IdentitySettingNames.User.IsUserNameUpdateEnabled), "true",
            StringComparison.OrdinalIgnoreCase);

        public bool IsEmailUpdateDisabled => !string.Equals(
            SettingManager.GetOrNull(IdentitySettingNames.User.IsEmailUpdateEnabled), "true",
            StringComparison.OrdinalIgnoreCase);


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

            ProfileDto user = null;

            if (IsUsernameUpdateDisabled )
            {
                user = await _profileAppService.GetAsync();
                updateDto.UserName = user.UserName;
            }

            if (IsEmailUpdateDisabled)
            {
                if (user == null)
                {
                    user = await _profileAppService.GetAsync();
                }

                updateDto.Email = user.Email;
            }

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