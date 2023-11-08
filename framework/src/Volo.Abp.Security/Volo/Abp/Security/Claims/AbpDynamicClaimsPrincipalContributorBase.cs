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
        await MapClaimsAsync(identity, dynamicClaims, identity.NameClaimType, "preferred_username");
        await MapClaimsAsync(identity, dynamicClaims, identity.NameClaimType, ClaimTypes.Name);
        await MapClaimsAsync(identity, dynamicClaims, identity.RoleClaimType, "role");
        await MapClaimsAsync(identity, dynamicClaims, identity.RoleClaimType, ClaimTypes.Role);
        await MapClaimsAsync(identity, dynamicClaims, "email", ClaimTypes.Email);
        await MapClaimsAsync(identity, dynamicClaims, "family_name", ClaimTypes.Surname);
        await MapClaimsAsync(identity, dynamicClaims, "given_name", ClaimTypes.GivenName);
    }

    protected virtual Task MapClaimsAsync(ClaimsIdentity identity, List<AbpClaimCacheItem> dynamicClaims, string sourceType, string targetType)
    {
        if (sourceType == targetType)
        {
            return Task.CompletedTask;;
        }

        if (identity.Claims.Any(c => c.Type == sourceType) && dynamicClaims.All(c => c.Type != sourceType))
        {
            var claims = dynamicClaims.Where(c => c.Type == targetType).ToList();
            if (!claims.IsNullOrEmpty())
            {
                identity.RemoveAll(sourceType);
                identity.AddClaims(claims.Select(c => new Claim(sourceType, c.Value)));
                dynamicClaims.RemoveAll(c => c.Type == targetType);
            }
        }

        if (identity.Claims.Any(c => c.Type == targetType) && dynamicClaims.All(c => c.Type != targetType))
        {
            var claims = dynamicClaims.Where(c => c.Type == sourceType).ToList();
            if (!claims.IsNullOrEmpty())
            {
                identity.RemoveAll(targetType);
                identity.AddClaims(claims.Select(c => new Claim(targetType, c.Value)));
                dynamicClaims.RemoveAll(c => c.Type == sourceType);
            }
        }

        return Task.CompletedTask;;
    }

    protected virtual Task AddDynamicClaims(ClaimsIdentity identity, List<AbpClaimCacheItem> dynamicClaims)
    {
        foreach (var claims in dynamicClaims.GroupBy(x => x.Type))
        {
            if (claims.Count() > 1)
            {
                identity.RemoveAll(claims.First().Type);
                identity.AddClaims(claims.Select(c => new Claim(claims.First().Type, c.Value)));
            }
            else
            {
                identity.AddOrReplace(new Claim(claims.First().Type, claims.First().Value));
            }
        }

        return Task.CompletedTask;;
    }
}
