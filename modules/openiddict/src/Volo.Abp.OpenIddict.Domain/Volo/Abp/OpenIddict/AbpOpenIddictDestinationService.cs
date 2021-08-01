using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict
{
    public class AbpOpenIddictDestinationService : IOpenIddictDestinationService, ITransientDependency
    {
        public virtual IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            if (claim.Type == AbpClaimTypes.TenantId)
            {
                yield return Destinations.AccessToken;
                yield return Destinations.IdentityToken;
                yield break;
            }

            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.ClientId:
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }

        public virtual Task SetDestinationsAsync(ClaimsPrincipal principal)
        {
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }
            return Task.CompletedTask;
        }
    }
}
