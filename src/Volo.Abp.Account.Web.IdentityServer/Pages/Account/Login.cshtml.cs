using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Ui;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class IdsLoginModel : AccountModelBase
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]
        public LoginInputModel LoginInput { get; set; }

        public bool EnableLocalLogin { get; set; }

        //TODO: Why there is an ExternalProviders if only the VisibleExternalProviders is used.
        public IEnumerable<ExternalProviderModel> ExternalProviders { get; set; }
        public IEnumerable<ExternalProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly AbpAccountOptions _accountOptions;
        private readonly IClientStore _clientStore;

        public IdsLoginModel(
            SignInManager<IdentityUser> signInManager, 
            IdentityUserManager userManager, 
            IIdentityServerInteractionService interaction, 
            IAuthenticationSchemeProvider schemeProvider, 
            IOptions<AbpAccountOptions> accountOptions, 
            IClientStore clientStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
            _accountOptions = accountOptions.Value;
        }

        public async Task OnGetAsync()
        {
            LoginInput = new LoginInputModel();

            var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            LoginInput.UserNameOrEmailAddress = context?.LoginHint;

            if (context?.IdP != null)
            {
                LoginInput.UserNameOrEmailAddress = context.LoginHint;
                ExternalProviders = new[] {new ExternalProviderModel {AuthenticationScheme = context.IdP}};
                return;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null || x.Name.Equals(_accountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                .Select(x => new ExternalProviderModel
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                })
                .ToList();

            EnableLocalLogin = true; //TODO: We can get default from a setting?
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    EnableLocalLogin = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }
            
            ExternalProviders = providers.ToArray();

            if (IsExternalLoginOnly)
            {
                //return await ExternalLogin(vm.ExternalLoginScheme, returnUrl);
                throw new NotImplementedException();
            }
        }

        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var result = await _signInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            if (!result.Succeeded)
            {
                throw new UserFriendlyException("Login failed!"); //TODO: Handle other cases, do not throw exception
            }

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }
        
        [UnitOfWork]
        public virtual IActionResult OnPostExternalLogin(string provider, string returnUrl = "", string returnUrlHash = "")
        {
            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { returnUrl, returnUrlHash });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [UnitOfWork]
        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string remoteError = null)
        {
            if (remoteError != null)
            {
                Logger.LogWarning($"External login callback error: {remoteError}");
                return RedirectToPage("./Login");
            }

            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                Logger.LogWarning("External login info is not available");
                return RedirectToPage("./Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(
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
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApplicationException("Error loading external login information during confirmation.");
            }

            var user = await CreateExternalUserAsync(info);

            await _signInManager.SignInAsync(user, false);
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        private async Task<IdentityUser> CreateExternalUserAsync(ExternalLoginInfo info)
        {
            var emailAddress = info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = new IdentityUser(GuidGenerator.Create(), emailAddress, CurrentTenant.Id);

            CheckIdentityErrors(await _userManager.CreateAsync(user));
            CheckIdentityErrors(await _userManager.SetEmailAsync(user, emailAddress));
            CheckIdentityErrors(await _userManager.AddLoginAsync(user, info));

            return user;
        }

        public class LoginInputModel
        {
            [Required]
            [StringLength(255)]
            [Display(Name = "UserNameOrEmailAddress")]
            public string UserNameOrEmailAddress { get; set; }

            [Required]
            [StringLength(32)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "RememberMe")]
            public bool RememberMe { get; set; }
        }

        public class ExternalProviderModel
        {
            public string DisplayName { get; set; }
            public string AuthenticationScheme { get; set; }
        }
    }
}
