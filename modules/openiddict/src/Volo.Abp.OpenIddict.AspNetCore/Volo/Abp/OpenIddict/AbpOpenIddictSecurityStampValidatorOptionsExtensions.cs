using System.Linq;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictSecurityStampValidatorOptionsExtensions
{
    public static SecurityStampValidatorOptions RemoveClientIdClaim(this SecurityStampValidatorOptions options)
    {
        // OpenIddictClaimsPrincipalContributor stamps the ambient /connect/authorize request's client_id
        // onto every principal built by CreateUserPrincipalAsync. That is meant for the access_token, but the
        // cookie security-stamp validator rebuilds the interactive cookie through the same method, so a refresh
        // that lands on /connect/authorize leaks client_id into the cookie and corrupts ICurrentClient.
        // OnRefreshingPrincipal is the only place the cookie is re-written and never runs for token issuance.
        var previousOnRefreshingPrincipal = options.OnRefreshingPrincipal;
        options.OnRefreshingPrincipal = async context =>
        {
            // Run the previous callback first: ABP Identity's UpdatePrincipal copies claims that are on the
            // current cookie but not on the new principal forward, re-introducing client_id from an
            // already-corrupted cookie. Removing it afterwards lets such a cookie self-heal on its next refresh.
            if (previousOnRefreshingPrincipal != null)
            {
                await previousOnRefreshingPrincipal.Invoke(context);
            }

            RemoveClientIdClaimsFromPrincipal(context);
        };

        return options;
    }

    private static void RemoveClientIdClaimsFromPrincipal(SecurityStampRefreshingPrincipalContext context)
    {
        if (context.NewPrincipal == null)
        {
            return;
        }

        foreach (var identity in context.NewPrincipal.Identities)
        {
            foreach (var claim in identity.FindAll(AbpClaimTypes.ClientId).ToArray())
            {
                identity.RemoveClaim(claim);
            }
        }
    }
}
