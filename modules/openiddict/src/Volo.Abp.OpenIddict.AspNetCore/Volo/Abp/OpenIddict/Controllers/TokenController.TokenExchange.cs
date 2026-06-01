using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.OpenIddict.Controllers;

public partial class TokenController
{
    protected virtual async Task<IActionResult> HandleTokenExchangeGrantTypeAsync(OpenIddictRequest request)
    {
        // Retrieve the claims principal stored in the subject token.
        //
        // Note: the principal may not represent a user (e.g if the token was issued during a client credentials token
        // request and represents a client application): developers are strongly encouraged to ensure that the user
        // and client identifiers are randomly generated so that a malicious client cannot impersonate a legit user.
        //
        // See https://datatracker.ietf.org/doc/html/rfc9068#SecurityConsiderations for more information.
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        // If available, retrieve the claims principal stored in the actor token.
        var actor = result.Properties?.GetParameter<ClaimsPrincipal>(OpenIddictServerAspNetCoreConstants.Properties.ActorTokenPrincipal);

        // Retrieve the user profile corresponding to the subject token.
        var user = await UserManager.FindByIdAsync(result.Principal!.GetClaim(OpenIddictConstants.Claims.Subject)!);
        if (user is null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                }));
        }

        // Ensure the user is still allowed to sign in.
        if (!await PreSignInCheckAsync(user))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                }));
        }

        // Note: whether the identity represents a delegated or impersonated access (or any other
        // model) is entirely up to the implementer: to support all scenarios, OpenIddict doesn't
        // enforce any specific constraint on the identity used for the sign-in operation and only
        // requires that the standard "act" and "may_act" claims be valid JSON objects if present.

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        // Create a new ClaimsPrincipal containing the claims that
        // will be used to create an id_token, a token or a code.
        var principal = await SignInManager.CreateUserPrincipalAsync(user);

        // Note: IdentityModel doesn't support serializing ClaimsIdentity.Actor to the
        // standard "act" claim yet, which requires adding the "act" claim manually.
        //
        // For more information, see
        // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3219.
        if (!string.IsNullOrEmpty(actor?.GetClaim(OpenIddictConstants.Claims.Subject)) &&
            !string.Equals(principal.GetClaim(OpenIddictConstants.Claims.Subject), actor.GetClaim(OpenIddictConstants.Claims.Subject), StringComparison.Ordinal))
        {
            principal.SetClaim(OpenIddictConstants.Claims.Actor, new JsonObject
            {
                [OpenIddictConstants.Claims.Subject] = actor.GetClaim(OpenIddictConstants.Claims.Subject)
            });
        }

        // Note: in this sample, the granted scopes match the requested scope
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        principal.SetScopes(request.GetScopes());
        principal.SetResources(await GetResourcesAsync(request.GetScopes()));

        await OpenIddictClaimsPrincipalManager.HandleAsync(request, principal);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
