using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public virtual async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        var userId = identity?.FindUserId();
        if (userId == null)
        {
            return;
        }

        var cache = context.GetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();
        var dynamicClaims = await cache.GetAsync(userId.Value, identity.FindTenantId());
        foreach (var claim in dynamicClaims)
        {
            identity.AddOrReplace(new Claim(claim.Type, claim.Value));
        }
    }
}
