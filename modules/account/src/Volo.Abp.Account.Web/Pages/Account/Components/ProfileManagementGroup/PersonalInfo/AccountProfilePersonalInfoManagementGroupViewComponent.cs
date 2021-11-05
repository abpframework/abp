using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo
{
    public class AccountProfilePersonalInfoManagementGroupViewComponent : AbpViewComponent
    {
        protected IProfileAppService ProfileAppService { get; }

        public AccountProfilePersonalInfoManagementGroupViewComponent(
            IProfileAppService profileAppService)
        {
            ProfileAppService = profileAppService;

            ObjectMapperContext = typeof(AbpAccountWebModule);
        }

        public virtual async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await ProfileAppService.GetAsync();

            var model = ObjectMapper.Map<ProfileDto, PersonalInfoModel>(user);

            return View("~/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.cshtml", model);
        }

        public class PersonalInfoModel : IHasConcurrencyStamp
        {
            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            [Display(Name = "DisplayName:UserName")]
            public string UserName { get; set; }

            [Required]
            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            [Display(Name = "DisplayName:Email")]
            public string Email { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
            [Display(Name = "DisplayName:Name")]
            public string Name { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
            [Display(Name = "DisplayName:Surname")]
            public string Surname { get; set; }

            [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
            [Display(Name = "DisplayName:PhoneNumber")]
            public string PhoneNumber { get; set; }

            [HiddenInput]
            public string ConcurrencyStamp { get; set; }
        }
    }
}
