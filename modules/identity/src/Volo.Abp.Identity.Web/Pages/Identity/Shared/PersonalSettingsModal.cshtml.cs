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

        private readonly IIdentityUserAppService _userAppService;

        public PersonalSettingsModal(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task OnGetAsync()
        {
            if (!CurrentUser.Id.HasValue)
            {
                throw new AuthenticationException();
            }

            var user = await _userAppService.GetAsync(CurrentUser.Id.Value);

            PersonalSettingsInfoModel = ObjectMapper.Map<IdentityUserDto, PersonalSettingsInfoModel>(user);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var updateDto = ObjectMapper.Map<PersonalSettingsInfoModel, UpdatePersonalSettingsDto>(PersonalSettingsInfoModel);

            await _userAppService.UpdatePersonalSettingsAsync(updateDto);

            return NoContent();
        }
    }

    public class PersonalSettingsInfoModel
    {
        [Required]
        [StringLength(IdentityUserConsts.MaxNameLength)]
        [Display(Name = "DisplayName:Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(IdentityUserConsts.MaxSurnameLength)]
        [Display(Name = "DisplayName:Surname")]
        public string Surname { get; set; }

        [Required]
        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        [Display(Name = "DisplayName:PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}