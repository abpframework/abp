using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public class AbpDefaultOpenIddictClaimDestinationsProvider : IAbpOpenIddictClaimDestinationsProvider, ITransientDependency
{
    public virtual Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context)
    {
        var securityStampClaimType = context
            .ScopeServiceProvider
            .GetRequiredService<IOptions<IdentityOptions>>().Value
            .ClaimsIdentity.SecurityStampClaimType;

        foreach (var claim in context.Claims)
        {
            if (claim.Type == AbpClaimTypes.TenantId)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
                continue;
            }

            switch (claim.Type)
            {
                case OpenIddictConstants.Claims.Name:
                    claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                    if (context.Principal.HasScope(OpenIddictConstants.Scopes.Profile))
                    {
                        claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
                    }
                    break;

                case OpenIddictConstants.Claims.Email:
                    claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                    if (context.Principal.HasScope(OpenIddictConstants.Scopes.Email))
                    {
                        claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
                    }
                    break;

                case OpenIddictConstants.Claims.Role:
                    claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                    if (context.Principal.HasScope(OpenIddictConstants.Scopes.Roles))
                    {
                        claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
                    }
                    break;

                default:
                    // Never include the security stamp in the access and identity tokens, as it's a secret value.
                    if (claim.Type != securityStampClaimType)
                    {
                        claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                    }
                    break;
            }
        }

        return Task.CompletedTask;
    }
}
