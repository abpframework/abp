using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
{
    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity == null)
        {
            return;
        }

        var userId = identity.FindUserId();
        if (userId == null)
        {
            return;
        }

        var dynamicClaimsCache = context.GetRequiredService<RemoteDynamicClaimsPrincipalContributorCache>();
        var dynamicClaims = await dynamicClaimsCache.GetAsync(userId.Value, identity.FindTenantId());

        await AddDynamicClaimsAsync(context, identity, dynamicClaims);
    }
}
