using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class LoginModel : AccountPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]
        public PostInput Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync()
        {
            ExternalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var result = await SignInManager.PasswordSignInAsync(
                Input.UserNameOrEmailAddress,
                Input.Password,
                Input.RememberMe,
                true
            );

            if (result.IsLockedOut)
            {
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./SendSecurityCode");
            }

            if (result.IsNotAllowed)
            {
                Alerts.Warning(L["LoginIsNotAllowed"]);
                return Page();
            }

            if (!result.Succeeded)
            {
                Alerts.Danger(L["InvalidUserNameOrPassword"]);
                return Page();
            }

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }
        
        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual IActionResult OnPostExternalLogin(string provider, string returnUrl = "", string returnUrlHash = "")
        {
            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { returnUrl, returnUrlHash });

            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string remoteError = null)
        {
            if (remoteError != null)
            {
                Logger.LogWarning($"External login callback error: {remoteError}");
                return RedirectToPage("./Login");
            }

            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                Logger.LogWarning("External login info is not available");
                return RedirectToPage("./Login");
            }

            var result = await SignInManager.ExternalLoginSignInAsync(
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );

            if (result.IsLockedOut)
            {
                throw new UserFriendlyException("Cannot proceed because user is locked out!");
            }

            //TODO: Handle other cases

            if (result.Succeeded)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            // Get the information about the user from the external login provider
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApplicationException("Error loading external login information during confirmation.");
            }

            var user = await CreateExternalUserAsync(info);

            await SignInManager.SignInAsync(user, false);
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        private async Task<IdentityUser> CreateExternalUserAsync(ExternalLoginInfo info)
        {
            var emailAddress = info.Principal.FindFirstValue(AbpClaimTypes.Email);

            var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);

            CheckIdentityErrors(await UserManager.CreateAsync(user));
            CheckIdentityErrors(await UserManager.SetEmailAsync(user, emailAddress));
            CheckIdentityErrors(await UserManager.AddLoginAsync(user, info));

            return user;
        }

        public class PostInput
        {
            [Required]
            [StringLength(255)]
            public string UserNameOrEmailAddress { get; set; }

            [Required]
            [StringLength(32)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            
            public bool RememberMe { get; set; }
        }
    }
}
