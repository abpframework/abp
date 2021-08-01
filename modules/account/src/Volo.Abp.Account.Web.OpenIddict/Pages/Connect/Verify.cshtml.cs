/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.OpenIddict;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.Account.Web.Pages.Connect
{
    [Authorize]
    public class VerifyModel : AbpOpenIddictPageModel
    {
        public VerifyViewModel ViewModel { get; set; }

        protected IOpenIddictApplicationManager ApplicationManager { get; }

        protected IOpenIddictScopeManager ScopeManager { get; }

        protected AbpSignInManager SignInManager { get; }

        protected IdentityUserManager UserManager { get; }

        protected IOpenIddictDestinationService OpenIddictDestinationService { get; }

        public VerifyModel(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            AbpSignInManager signInManager,
            IdentityUserManager userManager,
            IOpenIddictDestinationService openIddictDestinationService)
        {
            ApplicationManager = applicationManager;
            ScopeManager = scopeManager;
            SignInManager = signInManager;
            UserManager = userManager;
            OpenIddictDestinationService = openIddictDestinationService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            return await VerifyAsync();
        }

        public virtual async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "Accept")
            {
                return await AcceptAsync();
            }
            else if (action == "Deny")
            {
                return await DenyAsync();
            }
            return await VerifyAsync();
        }

        #region Device flow

        // Note: to support the device flow, you must provide your own verification endpoint action:
        protected virtual async Task<IActionResult> VerifyAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // If the user code was not specified in the query string (e.g as part of the verification_uri_complete),
            // render a form to ask the user to enter the user code manually (non-digit chars are automatically ignored).
            if (string.IsNullOrEmpty(request.UserCode))
            {
                ViewModel = new VerifyViewModel();
                return Page();
            }

            // Retrieve the claims principal associated with the user code.
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                // Retrieve the application details from the database using the client_id stored in the principal.
                var application = await ApplicationManager.FindByClientIdAsync(result.Principal.GetClaim(Claims.ClientId)) ??
                    throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

                // Render a form asking the user to confirm the authorization demand.
                ViewModel = new VerifyViewModel
                {
                    ApplicationName = await ApplicationManager.GetLocalizedDisplayNameAsync(application),
                    Scope = string.Join(" ", result.Principal.GetScopes()),
                    UserCode = request.UserCode
                };
                return Page();
            }

            // Redisplay the form when the user code is not valid.
            ViewModel = new VerifyViewModel
            {
                Error = Errors.InvalidToken,
                ErrorDescription = "The specified user code is not valid. Please make sure you typed it correctly."
            };
            return Page();
        }

        protected virtual async Task<IActionResult> AcceptAsync()
        {
            // Retrieve the profile of the logged in user.
            var user = await UserManager.GetUserAsync(User) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the claims principal associated with the user code.
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                var principal = await SignInManager.CreateUserPrincipalAsync(user);

                // TODO: allow use uncheck specific scopes

                // Note: in this sample, the granted scopes match the requested scope
                // but you may want to allow the user to uncheck specific scopes.
                // For that, simply restrict the list of scopes before calling SetScopes.
                principal.SetScopes(result.Principal.GetScopes());
                principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

                await OpenIddictDestinationService.SetDestinationsAsync(principal);

                var properties = new AuthenticationProperties
                {
                    // This property points to the address OpenIddict will automatically
                    // redirect the user to after validating the authorization demand.
                    RedirectUri = "/"
                };

                return SignIn(principal, properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Redisplay the form when the user code is not valid.
            ViewModel = new VerifyViewModel
            {
                Error = Errors.InvalidToken,
                ErrorDescription = "The specified user code is not valid. Please make sure you typed it correctly."
            };
            return Page();
        }

        // Notify OpenIddict that the authorization grant has been denied by the resource owner.
        protected virtual Task<IActionResult> DenyAsync()
        {
            return Task.FromResult<IActionResult>(
                Forbid(
                     authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                     properties: new AuthenticationProperties()
                     {
                         // This property points to the address OpenIddict will automatically
                         // redirect the user to after rejecting the authorization demand.
                         RedirectUri = "/"
                     })
                );
        }

        #endregion Device flow

        public class VerifyViewModel
        {
            [Display(Name = "Application")]
            public string ApplicationName { get; set; }

            [BindNever, Display(Name = "Error")]
            public string Error { get; set; }

            [BindNever, Display(Name = "Error description")]
            public string ErrorDescription { get; set; }

            [Display(Name = "Scope")]
            public string Scope { get; set; }

            [FromQuery(Name = OpenIddictConstants.Parameters.UserCode)]
            [Display(Name = "User code")]
            public string UserCode { get; set; }
        }
    }
}
