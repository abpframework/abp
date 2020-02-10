using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Settings;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class RegisterModel : AccountPageModel
    {
        private readonly IAccountAppService _accountAppService;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]
        public PostInput Input { get; set; }

        public RegisterModel(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        public virtual async Task OnGetAsync()
        {
            await CheckSelfRegistrationAsync();
        }

        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            await CheckSelfRegistrationAsync();

            var registerDto = new RegisterDto
            {
                AppName = "MVC",
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                UserName = Input.UserName
            };

            var userDto = await _accountAppService.RegisterAsync(registerDto);
            var user = await UserManager.GetByIdAsync(userDto.Id);

            await UserManager.SetEmailAsync(user, Input.EmailAddress);

            await SignInManager.SignInAsync(user, isPersistent: false);

            return Redirect(ReturnUrl ?? "/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }

        public class PostInput
        {
            [Required]
            [StringLength(IdentityUserConsts.MaxUserNameLength)]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(IdentityUserConsts.MaxEmailLength)]
            public string EmailAddress { get; set; }

            [Required]
            [StringLength(IdentityUserConsts.MaxPasswordLength)]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }
        }
    }
}
