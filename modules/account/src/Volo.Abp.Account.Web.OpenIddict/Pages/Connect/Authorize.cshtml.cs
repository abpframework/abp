/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Connect
{
    /// <summary>
    /// Authorization code, implicit and hybrid flows
    /// </summary>
    public class AuthorizeModel : AbpOpenIddictPageModel
    {
        public AuthorizeViewModel ViewModel { get; set; }

        protected IdentityUserManager UserManager { get; }

        protected AbpSignInManager SignInManager { get; }

        protected IOpenIddictApplicationManager ApplicationManager { get; }

        protected IOpenIddictAuthorizationManager AuthorizationManager { get; }

        protected IOpenIddictScopeManager ScopeManager { get; }

        protected IOpenIddictDestinationService OpenIddictDestinationService { get; }

        public AuthorizeModel(
            IdentityUserManager userManager,
            AbpSignInManager signInManager,
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager,
            IOpenIddictDestinationService openIddictDestinationService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            ApplicationManager = applicationManager;
            AuthorizationManager = authorizationManager;
            ScopeManager = scopeManager;
            OpenIddictDestinationService = openIddictDestinationService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            return await AuthorizeAsync(request);
        }

        public virtual async Task<IActionResult> OnPostAsync(string action)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (action == "Accept")
            {
                return await AcceptAsync(request);
            }
            else if (action == "Deny")
            {
                return await DenyAsync();
            }

            return await AuthorizeAsync(request);
        }

        protected virtual async Task<IActionResult> AuthorizeAsync(OpenIddictRequest request)
        {
            // Retrieve the user principal stored in the authentication cookie.
            // If it can't be extracted, redirect the user to the login page.
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            if (result is null || !result.Succeeded)
            {
                // If the client application requested promptless authentication,
                // return an error indicating that the user is not logged in.
                if (request.HasPrompt(Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }));
                }

                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            // If prompt=login was specified by the client application,
            // immediately return the user agent to the login page.
            if (request.HasPrompt(Prompts.Login))
            {
                // To avoid endless login -> authorization redirects, the prompt=login flag
                // is removed from the authorization request payload before redirecting the user.
                var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));

                var parameters = Request.HasFormContentType ?
                    Request.Form.Where(parameter => parameter.Key != Parameters.Prompt).ToList() :
                    Request.Query.Where(parameter => parameter.Key != Parameters.Prompt).ToList();

                parameters.Add(KeyValuePair.Create(Parameters.Prompt, new StringValues(prompt)));

                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(parameters)
                    });
            }

            // If a max_age parameter was provided, ensure that the cookie is not too old.
            // If it's too old, automatically redirect the user agent to the login page.
            if (request.MaxAge is not null && result.Properties?.IssuedUtc is not null &&
                DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value))
            {
                if (request.HasPrompt(Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }));
                }

                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            // Retrieve the profile of the logged in user.
            var user = await UserManager.GetUserAsync(result.Principal) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await ApplicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await AuthorizationManager.FindAsync(
                subject: await UserManager.GetUserIdAsync(user),
                client: await ApplicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            switch (await ApplicationManager.GetConsentTypeAsync(application))
            {
                // If the consent is external (e.g when authorizations are granted by a sysadmin),
                // immediately return an error if no authorization can be found in the database.
                case ConsentTypes.External when !authorizations.Any():
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The logged in user is not allowed to access this client application."
                        }));

                // If the consent is implicit or if an authorization was found,
                // return an authorization response without displaying the consent form.
                case ConsentTypes.Implicit:
                case ConsentTypes.External when authorizations.Any():
                case ConsentTypes.Explicit when authorizations.Any() && !request.HasPrompt(Prompts.Consent):
                    return await ReturnSuccessResultAsync(request, user, authorizations, application);

                // At this point, no authorization was found in the database and an error must be returned
                // if the client application specified prompt=none in the authorization request.
                case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "Interactive user consent is required."
                        }));

                // In every other case, render the consent form.
                default:
                    ViewModel = new AuthorizeViewModel
                    {
                        ApplicationName = await ApplicationManager.GetLocalizedDisplayNameAsync(application),
                        Scope = request.Scope
                    };
                    return Page();
            }
        }

        protected virtual async Task<IActionResult> AcceptAsync(OpenIddictRequest request)
        {
            // Retrieve the profile of the logged in user.
            var user = await UserManager.GetUserAsync(User) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await ApplicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await AuthorizationManager.FindAsync(
                subject: await UserManager.GetUserIdAsync(user),
                client: await ApplicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            // Note: the same check is already made in the other action but is repeated
            // here to ensure a malicious user can't abuse this POST-only endpoint and
            // force it to return a valid response without the external authorization.
            if (!authorizations.Any() && await ApplicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }));
            }

            return await ReturnSuccessResultAsync(request, user, authorizations, application);
        }

        // Notify OpenIddict that the authorization grant has been denied by the resource owner
        // to redirect the user agent to the client application using the appropriate response_mode.
        protected virtual async Task<IActionResult> DenyAsync()
        {
            await Task.CompletedTask;

            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        protected virtual async Task<IActionResult> ReturnSuccessResultAsync(
            OpenIddictRequest request,
            IdentityUser user, 
            List<object> authorizations,
            object application)
        {
            var principal = await SignInManager.CreateUserPrincipalAsync(user);

            // TODO: allow use uncheck specific scopes

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            principal.SetScopes(request.GetScopes());
            principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            // Automatically create a permanent authorization to avoid requiring explicit consent
            // for future authorization or token requests containing the same scopes.
            var authorization = authorizations.LastOrDefault();
            if (authorization is null)
            {
                authorization = await AuthorizationManager.CreateAsync(
                    principal: principal,
                    subject: await UserManager.GetUserIdAsync(user),
                    client: await ApplicationManager.GetIdAsync(application),
                    type: AuthorizationTypes.Permanent,
                    scopes: principal.GetScopes());
            }

            principal.SetAuthorizationId(await AuthorizationManager.GetIdAsync(authorization));

            await OpenIddictDestinationService.SetDestinationsAsync(principal);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        public class AuthorizeViewModel
        {
            [Display(Name = "Application")]
            public string ApplicationName { get; set; }

            [Display(Name = "Scope")]
            public string Scope { get; set; }
        }
    }
}
