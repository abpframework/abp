using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    [ExposeServices(typeof(LoginModel))]
    public class IdentityServerSupportedLoginModel : LoginModel
    {
        protected IIdentityServerInteractionService Interaction { get; }
        protected IClientStore ClientStore { get; }
        protected IEventService IdentityServerEvents { get; }

        public IdentityServerSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions, 
            IIdentityServerInteractionService interaction, 
            IClientStore clientStore, 
            IEventService identityServerEvents)
            :base(
                schemeProvider, 
                accountOptions)
        {
            Interaction = interaction;
            ClientStore = clientStore;
            IdentityServerEvents = identityServerEvents;
        }

        public override async Task OnGetAsync()
        {
            LoginInput = new LoginInputModel();

            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null)
            {
                LoginInput.UserNameOrEmailAddress = context.LoginHint;

                //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
                var tenant = context.Parameters[TenantResolverConsts.DefaultTenantKey];
                if (!string.IsNullOrEmpty(tenant))
                {
                    CurrentTenant.Change(Guid.Parse(tenant));
                    Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
                }
            }

            if (context?.IdP != null)
            {
                LoginInput.UserNameOrEmailAddress = context.LoginHint;
                ExternalProviders = new[] { new ExternalProviderModel { AuthenticationScheme = context.IdP } };
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
                var client = await ClientStore.FindEnabledClientByIdAsync(context.ClientId);
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
        public override async Task<IActionResult> OnPostAsync(string action)
        {
            EnableLocalLogin = true; //TODO: We can get default from a setting?

            if (action == "Cancel")
            {
                var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
                if (context == null)
                {
                    return Redirect("~/");
                }

                await Interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                return Redirect(ReturnUrl);
            }

            ValidateModel();

            await ReplaceEmailToUsernameOfInputIfNeeds();

            var result = await SignInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./SendSecurityCode", new
                {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash,
                    rememberMe = LoginInput.RememberMe
                });
            }

            if (result.IsLockedOut)
            {
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
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

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                       await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

            Debug.Assert(user != null, nameof(user) + " != null");
            await IdentityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

            return RedirectSafely(ReturnUrl, ReturnUrlHash);
        }

        [UnitOfWork]
        public override async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (_accountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            return await base.OnPostExternalLogin(provider);
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
    }
}
