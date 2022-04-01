using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Volo.Abp.OpenIddict.Controllers;

public partial class TokenController
{
    protected virtual async Task<IActionResult> HandleClientCredentialsAsync(OpenIddictRequest request)
    {
        // Note: the client credentials are automatically validated by OpenIddict:
        // if client_id or client_secret are invalid, this action won't be invoked.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId);
        if (application == null)
        {
            throw new InvalidOperationException(L["TheApplicationDetailsCannotBeFound"]);
        }

        // Create a new ClaimsIdentity containing the claims that
        // will be used to create an id_token, a token or a code.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

        // Use the client_id as the subject identifier.
        identity.AddClaim(OpenIddictConstants.Claims.Subject, await ApplicationManager.GetClientIdAsync(application),
            OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);

        identity.AddClaim(OpenIddictConstants.Claims.Name, await ApplicationManager.GetDisplayNameAsync(application),
            OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);

        // Note: In the original OAuth 2.0 specification, the client credentials grant
        // doesn't return an identity token, which is an OpenID Connect concept.
        //
        // As a non-standardized extension, OpenIddict allows returning an id_token
        // to convey information about the client application when the "openid" scope
        // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
        // scope is not explicitly set, no identity token is returned to the client application.

        // Set the list of scopes granted to the client application in access_token.
        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(request.GetScopes());
        principal.SetResources(await GetResourcesAsync(request.GetScopes()));

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(claim));
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }


    protected virtual IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        return claim.Type switch {
            OpenIddictConstants.Claims.Name or OpenIddictConstants.Claims.Subject
                => ImmutableArray.Create(OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken),

            _ => ImmutableArray.Create(OpenIddictConstants.Destinations.AccessToken)
        };
    }
}
