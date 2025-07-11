using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.AspNetCore.Security;
using Volo.Abp.OpenIddict.ViewModels.Authorization;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict.Controllers;

[Route("connect/authorize")]
[ApiExplorerSettings(IgnoreApi = true)]
public class AuthorizeController : AbpOpenIdDictControllerBase
{
    [HttpGet, HttpPost]
    [IgnoreAntiforgeryToken]
    [IgnoreAbpSecurityHeader]
    public virtual async Task<IActionResult> HandleAsync()
    {
        var request = await GetOpenIddictServerRequestAsync(HttpContext);

        // Try to retrieve the user principal stored in the authentication cookie and redirect
        // the user agent to the login page (or to an external provider) in the following cases:
        //
        //  - If the user principal can't be extracted or the cookie is too old.
        //  - If prompt=login was specified by the client application.
        //  - If max_age=0 was specified by the client application (max_age=0 is equivalent to prompt=login).
        //  - If a max_age parameter was provided and the authentication cookie is not considered "fresh" enough.
        //
        // For scenarios where the default authentication handler configured in the ASP.NET Core
        // authentication options shouldn't be used, a specific scheme can be specified here.
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (result is not { Succeeded: true } ||
            ((request.HasPromptValue(OpenIddictConstants.PromptValues.Login) || request.MaxAge is 0 ||
              (request.MaxAge != null && result.Properties?.IssuedUtc != null &&
               TimeProvider.System.GetUtcNow() - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value))) &&
             TempData["IgnoreAuthenticationChallenge"] is null or false))
        {
            // If the client application requested promptless authentication,
            // return an error indicating that the user is not logged in.
            if (request.HasPromptValue(OpenIddictConstants.PromptValues.None))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.LoginRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                    }));
            }

            // To avoid endless login endpoint -> authorization endpoint redirects, a special temp data entry is
            // used to skip the challenge if the user agent has already been redirected to the login endpoint.
            //
            // Note: this flag doesn't guarantee that the user has accepted to re-authenticate. If such a guarantee
            // is needed, the existing authentication cookie MUST be deleted AND revoked (e.g using ASP.NET Core
            // Identity's security stamp feature with an extremely short revalidation time span) before triggering
            // a challenge to redirect the user agent to the login endpoint.
            TempData["IgnoreAuthenticationChallenge"] = true;

            // For scenarios where the default challenge handler configured in the ASP.NET Core
            // authentication options shouldn't be used, a specific scheme can be specified here.
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Request.PathBase + Request.Path + QueryString.Create(Request.HasFormContentType ? Request.Form : Request.Query)
            });
        }

        // If prompt=select_account was specified by the client application,
        // We will redirect the user to the select_account page.
        if (request.HasPromptValue(OpenIddictConstants.PromptValues.SelectAccount) && TempData["IgnoreSelectAccount"] is null or false)
        {
            // To avoid endless select account endpoint -> authorization endpoint redirects, a special temp data entry is
            // used to skip the redirect if the user agent has already been redirected to the select account endpoint.
            TempData["IgnoreSelectAccount"] = true;

            var selectAccountPath = HttpContext.RequestServices.GetRequiredService<IOptions<AbpOpenIddictAspNetCoreOptions>>().Value.SelectAccountPage.RemovePostFix("/");
            return Redirect(Url.Content($"{selectAccountPath}?RedirectUri={UrlEncoder.Default.Encode(Request.PathBase + Request.Path + QueryString.Create(Request.HasFormContentType ? Request.Form : Request.Query))}"));
        }

        // Retrieve the profile of the logged in user.
        var dynamicPrincipal = result.Principal;
        if (AbpClaimsPrincipalFactoryOptions.Value.IsDynamicClaimsEnabled)
        {
            dynamicPrincipal = await AbpClaimsPrincipalFactory.CreateDynamicAsync(dynamicPrincipal);
            if (dynamicPrincipal == null)
            {
                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(Request.HasFormContentType ? Request.Form : Request.Query)
                    });
            }
        }

        var user = await UserManager.GetUserAsync(dynamicPrincipal);
        if (user == null)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(Request.HasFormContentType ? Request.Form : Request.Query)
                });
        }

        // Retrieve the application details from the database.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException(L["DetailsConcerningTheCallingClientApplicationCannotBeFound"]);

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await AuthorizationManager.FindAsync(
            subject: await UserManager.GetUserIdAsync(user),
            client: await ApplicationManager.GetIdAsync(application),
            status: OpenIddictConstants.Statuses.Valid,
            type: OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes: request.GetScopes()).ToListAsync();

        switch (await ApplicationManager.GetConsentTypeAsync(application))
        {
            // If the consent is external (e.g when authorizations are granted by a sysadmin),
            // immediately return an error if no authorization can be found in the database.
            case OpenIddictConstants.ConsentTypes.External when !authorizations.Any():
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The logged in user is not allowed to access this client application."
                    }));

            // If the consent is implicit or if an authorization was found,
            // return an authorization response without displaying the consent form.
            case OpenIddictConstants.ConsentTypes.Implicit:
            case OpenIddictConstants.ConsentTypes.External when authorizations.Any():
            case OpenIddictConstants.ConsentTypes.Explicit when authorizations.Any() && !request.HasPromptValue(OpenIddictConstants.PromptValues.Consent):
                var principal = await SignInManager.CreateUserPrincipalAsync(user);

                var sid = dynamicPrincipal.FindFirst(JwtRegisteredClaimNames.Sid);
                if (sid != null)
                {
                    principal.RemoveClaims(JwtRegisteredClaimNames.Sid);
                    principal.AddClaim(JwtRegisteredClaimNames.Sid, sid.Value);
                }

                if (result.Properties != null && result.Properties.IsPersistent)
                {
                    var claim = new Claim(AbpClaimTypes.RememberMe, true.ToString()).SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                    principal.Identities.FirstOrDefault()?.AddClaim(claim);
                }

                // Note: in this sample, the granted scopes match the requested scope
                // but you may want to allow the user to uncheck specific scopes.
                // For that, simply restrict the list of scopes before calling SetScopes.
                principal.SetScopes(request.GetScopes());
                principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

                // Automatically create a permanent authorization to avoid requiring explicit consent
                // for future authorization or token requests containing the same scopes.
                var authorization = authorizations.LastOrDefault();
                if (authorization == null)
                {
                    authorization = await AuthorizationManager.CreateAsync(
                        principal: principal,
                        subject: await UserManager.GetUserIdAsync(user),
                        client: await ApplicationManager.GetIdAsync(application),
                        type: OpenIddictConstants.AuthorizationTypes.Permanent,
                        scopes: principal.GetScopes());
                }

                principal.SetAuthorizationId(await AuthorizationManager.GetIdAsync(authorization));

                await OpenIddictClaimsPrincipalManager.HandleAsync(request, principal);

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // At this point, no authorization was found in the database and an error must be returned
            // if the client application specified prompt=none in the authorization request.
            case OpenIddictConstants.ConsentTypes.Explicit when request.HasPromptValue(OpenIddictConstants.PromptValues.None):
            case OpenIddictConstants.ConsentTypes.Systematic when request.HasPromptValue(OpenIddictConstants.PromptValues.None):
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Interactive user consent is required."
                    }));

            // In every other case, render the consent form.
            default:
                return View("Authorize", new AuthorizeViewModel
                {
                    ApplicationName = await ApplicationManager.GetDisplayNameAsync(application),
                    Scope = request.Scope
                });
        }
    }

    [HttpPost]
    [Authorize]
    [Route("callback")]
    public virtual async Task<IActionResult> HandleCallbackAsync()
    {
        if (await HasFormValueAsync("deny"))
        {
            // Notify OpenIddict that the authorization grant has been denied by the resource owner
            // to redirect the user agent to the client application using the appropriate response_mode.
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var request = await GetOpenIddictServerRequestAsync(HttpContext);

        // Retrieve the profile of the logged in user.
        var user = await UserManager.GetUserAsync(User) ??
                   throw new InvalidOperationException(L["TheUserDetailsCannotBbeRetrieved"]);

        // Retrieve the application details from the database.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException(L["DetailsConcerningTheCallingClientApplicationCannotBeFound"]);

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await AuthorizationManager.FindAsync(
            subject: await UserManager.GetUserIdAsync(user),
            client : await ApplicationManager.GetIdAsync(application),
            status : OpenIddictConstants.Statuses.Valid,
            type   : OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes : request.GetScopes()).ToListAsync();

        // Note: the same check is already made in the other action but is repeated
        // here to ensure a malicious user can't abuse this POST-only endpoint and
        // force it to return a valid response without the external authorization.
        if (!authorizations.Any() && await ApplicationManager.HasConsentTypeAsync(application, OpenIddictConstants.ConsentTypes.External))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.ConsentRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The logged in user is not allowed to access this client application."
                }));
        }

        var principal = await SignInManager.CreateUserPrincipalAsync(user);

        var sid = User.FindFirst(JwtRegisteredClaimNames.Sid);
        if (sid != null)
        {
            principal.RemoveClaims(JwtRegisteredClaimNames.Sid);
            principal.AddClaim(JwtRegisteredClaimNames.Sid, sid.Value);
        }

        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (result.Succeeded && result.Properties != null && result.Properties.IsPersistent)
        {
            var claim = new Claim(AbpClaimTypes.RememberMe, true.ToString()).SetDestinations(OpenIddictConstants.Destinations.AccessToken);
            principal.Identities.FirstOrDefault()?.AddClaim(claim);
        }

        // Note: in this sample, the granted scopes match the requested scope
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        principal.SetScopes(request.GetScopes());
        principal.SetResources(await ScopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

        // Automatically create a permanent authorization to avoid requiring explicit consent
        // for future authorization or token requests containing the same scopes.
        var authorization = authorizations.LastOrDefault();
        if (authorization == null)
        {
            authorization = await AuthorizationManager.CreateAsync(
                principal: principal,
                subject  : await UserManager.GetUserIdAsync(user),
                client   : await ApplicationManager.GetIdAsync(application),
                type     : OpenIddictConstants.AuthorizationTypes.Permanent,
                scopes   : principal.GetScopes());
        }

        principal.SetAuthorizationId(await AuthorizationManager.GetIdAsync(authorization));
        principal.SetScopes(request.GetScopes());
        principal.SetResources(await GetResourcesAsync(request.GetScopes()));

        await OpenIddictClaimsPrincipalManager.HandleAsync(request, principal);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
