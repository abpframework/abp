using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict.Localization;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.OpenIddict.Controllers;

public abstract class OpenIdDictControllerBase : AbpController
{
    protected SignInManager<IdentityUser> SignInManager => LazyServiceProvider.LazyGetRequiredService<SignInManager<IdentityUser>>();
    protected IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();
    protected IOpenIddictApplicationManager ApplicationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictApplicationManager>();
    protected IOpenIddictAuthorizationManager AuthorizationManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictAuthorizationManager>();
    protected IOpenIddictScopeManager ScopeManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictScopeManager>();
    protected IOpenIddictTokenManager TokenManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictTokenManager>();

    protected OpenIdDictControllerBase()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
    }

    protected virtual IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case OpenIddictConstants.Claims.Name:
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (principal.HasScope(OpenIddictConstants.Scopes.Profile))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }

                yield break;

            case OpenIddictConstants.Claims.Email:
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (principal.HasScope(OpenIddictConstants.Scopes.Email))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }

                yield break;

            case OpenIddictConstants.Claims.Role:
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (principal.HasScope(OpenIddictConstants.Scopes.Roles))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return OpenIddictConstants.Destinations.AccessToken;
                yield break;
        }
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
            _ => ImmutableArray.Create(OpenIddictConstants.Destinations.AccessToken),
        };
    }
}
