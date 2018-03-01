using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web.Auth;
using Volo.Abp.Identity;
using Volo.Abp.Ui;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    //TODO: Inherit from LoginModel of Account.Web project. We should design it as extensible.
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
        private readonly IEventService _identityServerEvents;

        public IdsLoginModel(
            SignInManager<IdentityUser> signInManager, 
            IdentityUserManager userManager, 
            IIdentityServerInteractionService interaction, 
            IAuthenticationSchemeProvider schemeProvider, 
            IOptions<AbpAccountOptions> accountOptions, 
            IClientStore clientStore, 
            IEventService identityServerEvents)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
            _identityServerEvents = identityServerEvents;
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
        public virtual async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "Cancel")
            {
                var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
                if (context == null)
                {
                    return Redirect("~/");
                }

                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
                return RedirectSafely(ReturnUrl);
            }

            ValidateModel();

            var result = await _signInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await _userManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                       await _userManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

            if (!result.Succeeded)
            {
                await _identityServerEvents.RaiseAsync(new UserLoginFailureEvent(LoginInput.UserNameOrEmailAddress, "Login failed: " + result.GetResultAsString()));
                throw new UserFriendlyException("Login failed!"); //TODO: Handle other cases, do not throw exception
            }

            Debug.Assert(user != null, nameof(user) + " != null");
            await _identityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

            if (!_interaction.IsValidReturnUrl(ReturnUrl))
            {
                return Redirect("~/");
            }

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }
        
        [UnitOfWork]
        public virtual async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (_accountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["scheme"] = provider;

            return Challenge(properties, provider);
        }

        [UnitOfWork]
        public virtual async Task<IActionResult> OnGetExternalLoginCallbackAsync(string returnUrl = "", string returnUrlHash = "", string remoteError = null)
        {
            //TODO: Did not implemented Identity Server 4 sample for this method (see ExternalLoginCallback in Quickstart of IDS4 sample)
            /* Also did not implement these:
             * - Logout(string logoutId)
             */

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

            if (result.Succeeded)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            //TODO: Handle other cases for result!

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

        private async Task<IActionResult> ProcessWindowsLoginAsync()
        {
            var result = await HttpContext.AuthenticateAsync(_accountOptions.WindowsAuthenticationSchemeName);
            if (!(result?.Principal is WindowsPrincipal windowsPrincipal))
            {
                return Challenge(_accountOptions.WindowsAuthenticationSchemeName);
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                Items =
                {
                    {"scheme", _accountOptions.WindowsAuthenticationSchemeName},
                }
            };

            var identity = new ClaimsIdentity(_accountOptions.WindowsAuthenticationSchemeName);
            identity.AddClaim(new Claim(JwtClaimTypes.Subject, windowsPrincipal.Identity.Name));
            identity.AddClaim(new Claim(JwtClaimTypes.Name, windowsPrincipal.Identity.Name));

            //TODO: Consider to add Windows groups the the identity
            //if (_accountOptions.IncludeWindowsGroups)
            //{
            //    var windowsIdentity = windowsPrincipal.Identity as WindowsIdentity;
            //    if (windowsIdentity != null)
            //    {
            //        var groups = windowsIdentity.Groups?.Translate(typeof(NTAccount));
            //        var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
            //        identity.AddClaims(roles);
            //    }
            //}

            await HttpContext.SignInAsync(
                IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                new ClaimsPrincipal(identity),
                props
            );

            return RedirectSafely(props.RedirectUri);
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
