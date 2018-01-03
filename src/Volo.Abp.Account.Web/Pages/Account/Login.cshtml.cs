using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Ui;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class LoginModel : AccountModelBase
    {
        public string ReturnUrl { get; set; }  //TODO: Try to automatically bind from querystring!

        public string ReturnUrlHash { get; set; }  //TODO: Try to automatically bind from querystring!

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager; //TODO: We should not use domain from presentation..?

        public LoginModel(SignInManager<IdentityUser> signInManager, IdentityUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task OnGetAsync(string returnUrl = "", string returnUrlHash = "")
        {
            ReturnUrl = returnUrl;
            ReturnUrlHash = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        //TODO: Bind input to a property instead of getting as parameter..?
        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnPostAsync(PostInput input, string returnUrl = "", string returnUrlHash = "")
        {
            ReturnUrl = returnUrl;
            ReturnUrlHash = returnUrl;

            if (!ModelState.IsValid)
            {
                throw new NotImplementedException();
            }

            var result = await _signInManager.PasswordSignInAsync(
                input.UserNameOrEmailAddress,
                input.Password,
                input.RememberMe,
                true
            );

            if (!result.Succeeded)
            {
                throw new UserFriendlyException("Login failed!"); //TODO: Handle other cases, do not throw exception
            }

            //TODO: Use LocalRedirect and Url.GetLocalUrl methods instead of a custom one!
            return RedirectSafely(returnUrl, returnUrlHash);
        }
        
        [UnitOfWork]
        public virtual IActionResult OnPostExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [UnitOfWork]
        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = null, string returnUrlHash = "", string remoteError = null)
        {
            if (remoteError != null)
            {
                return RedirectToPage("./Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            if (result.IsLockedOut)
            {
                throw new UserFriendlyException("Cannot proceed because user is locked out!");
            }

            ReturnUrl = returnUrl;

            //User does not have an account, create an account.
            await CreateUserAsync();
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        [UnitOfWork]
        protected virtual async Task CreateUserAsync()
        {
            ValidateModel();

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApplicationException("Error loading external login information during confirmation.");
            }

            var emailAddress = info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = new IdentityUser(GuidGenerator.Create(), emailAddress);

            CheckIdentityErrors(await _userManager.CreateAsync(user));
            CheckIdentityErrors(await _userManager.SetEmailAsync(user, emailAddress));
            CheckIdentityErrors(await _userManager.AddLoginAsync(user, info));

            await _signInManager.SignInAsync(user, false);
        }

        public class PostInput
        {
            [Required]
            [StringLength(255)]
            public string UserNameOrEmailAddress { get; set; }

            [Required]
            [StringLength(32)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}
