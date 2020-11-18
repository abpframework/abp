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
using Volo.Abp.Account.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

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

        public async override Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginInputModel();

            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null)
            {
                ShowCancelButton = true;

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
                return Page();
            }

            var providers = await GetExternalProviders();
            ExternalProviders = providers.ToList();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            if (context?.Client?.ClientId != null)
            {
                var client = await ClientStore.FindEnabledClientByIdAsync(context?.Client?.ClientId);
                if (client != null)
                {
                    EnableLocalLogin = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            if (IsExternalLoginOnly)
            {
                return await base.OnPostExternalLogin(providers.First().AuthenticationScheme);
            }

            return Page();
        }

        public async override Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "Cancel")
            {
                var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
                if (context == null)
                {
                    return Redirect("~/");
                }

                await Interaction.GrantConsentAsync(context, new ConsentResponse()
                {
                    Error = AuthorizationError.AccessDenied
                });

                return Redirect(ReturnUrl);
            }

            await CheckLocalLoginAsync();

            ValidateModel();

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            await ReplaceEmailToUsernameOfInputIfNeeds();

            var result = await SignInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = LoginInput.UserNameOrEmailAddress
            });

            if (result.RequiresTwoFactor)
            {
                return await TwoFactorLoginResultAsync();
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

        public async override Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            return await base.OnPostExternalLogin(provider);
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync()
        {
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (!(result?.Principal is WindowsPrincipal windowsPrincipal))
            {
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                Items =
                {
                    {"scheme", AccountOptions.WindowsAuthenticationSchemeName},
                }
            };

            var identity = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
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
