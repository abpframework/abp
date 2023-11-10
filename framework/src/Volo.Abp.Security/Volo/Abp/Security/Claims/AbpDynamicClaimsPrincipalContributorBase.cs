using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims;

public abstract class AbpDynamicClaimsPrincipalContributorBase : IAbpDynamicClaimsPrincipalContributor, ITransientDependency
{
    public abstract Task ContributeAsync(AbpClaimsPrincipalContributorContext context);

    protected virtual async Task MapCommonClaimsAsync(ClaimsIdentity identity, List<AbpClaimCacheItem> dynamicClaims)
    {
        await MapClaimAsync(identity, dynamicClaims, AbpClaimTypes.UserName, "preferred_username", "unique_name", ClaimTypes.Name);
        await MapClaimAsync(identity, dynamicClaims, AbpClaimTypes.Role, "role", "roles", ClaimTypes.Role);
        await MapClaimAsync(identity, dynamicClaims, AbpClaimTypes.Email, "email", ClaimTypes.Email);
        await MapClaimAsync(identity, dynamicClaims, AbpClaimTypes.SurName, "family_name", ClaimTypes.Surname);
        await MapClaimAsync(identity, dynamicClaims, AbpClaimTypes.Name, "given_name", ClaimTypes.GivenName);
    }

    protected virtual Task MapClaimAsync(ClaimsIdentity identity, List<AbpClaimCacheItem> dynamicClaims, string abpClaimType, params string[] dynamicClaimTypes)
    {
        var claims = dynamicClaims.Where(c => dynamicClaimTypes.Contains(c.Type)).ToList();
        if (claims.IsNullOrEmpty())
        {
            return Task.CompletedTask;
        }

        foreach (var claimGroup in claims.GroupBy(x => x.Type))
        {
            var claim = claimGroup.First();
            if (claimGroup.Count() > 1)
            {
                dynamicClaims.RemoveAll(c => c.Type == claim.Type && identity.Claims.All(x => x.Type != claim.Type));
                identity.RemoveAll(abpClaimType);
                identity.AddClaims(claimGroup.Where(c => c.Value != null).Select(c => new Claim(abpClaimType, c.Value!)));
            }
            else
            {
                dynamicClaims.RemoveAll(c => c.Type == claim.Type && identity.Claims.All(x => x.Type != claim.Type));
                if (claim.Value != null)
                {
                    identity.AddOrReplace(new Claim(abpClaimType, claim.Value));
                }
                else
                {
                    identity.RemoveAll(abpClaimType);
                }
            }
        }

        return Task.CompletedTask;;
    }

    protected virtual Task AddDynamicClaims(ClaimsIdentity identity, List<AbpClaimCacheItem> dynamicClaims)
    {
        foreach (var claimGroup in dynamicClaims.GroupBy(x => x.Type))
        {
            if (claimGroup.Count() > 1)
            {
                identity.RemoveAll(claimGroup.First().Type);
                identity.AddClaims(claimGroup.Where(c => c.Value != null).Select(c => new Claim(claimGroup.First().Type, c.Value!)));
            }
            else
            {
                var claim = claimGroup.First();
                if (claim.Value != null)
                {
                    identity.AddOrReplace(new Claim(claimGroup.First().Type, claim.Value));
                }
                else
                {
                    identity.RemoveAll(claim.Type);
                }
            }
        }

        return Task.CompletedTask;;
    }
}
