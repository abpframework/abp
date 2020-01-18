using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class RegisterModel : AccountPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]
        public PostInput Input { get; set; }

        public virtual async Task OnGet()
        {
            await CheckSelfRegistrationAsync().ConfigureAwait(false);
        }

        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            await CheckSelfRegistrationAsync().ConfigureAwait(false);

            var user = new IdentityUser(GuidGenerator.Create(), Input.UserName, Input.EmailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user, Input.Password).ConfigureAwait(false)).CheckErrors();

            await UserManager.SetEmailAsync(user, Input.EmailAddress).ConfigureAwait(false);

            await SignInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);

            return Redirect(ReturnUrl ?? "/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled).ConfigureAwait(false) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin).ConfigureAwait(false))
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
            public string Password { get; set; }
        }
    }
}
