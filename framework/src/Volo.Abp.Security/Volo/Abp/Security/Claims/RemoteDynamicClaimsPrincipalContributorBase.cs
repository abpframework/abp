using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Security.Claims;

public abstract class RemoteDynamicClaimsPrincipalContributorBase<TContributor, TContributorCache> : AbpDynamicClaimsPrincipalContributorBase
    where TContributor : class
    where TContributorCache : RemoteDynamicClaimsPrincipalContributorCacheBase<TContributorCache>
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

        var dynamicClaimsCache = context.GetRequiredService<TContributorCache>().As<TContributorCache>();
        AbpDynamicClaimCacheItem dynamicClaims;
        try
        {
            dynamicClaims = await dynamicClaimsCache.GetAsync(userId.Value, identity.FindTenantId());
        }
        catch (Exception e)
        {
            // In case if failed refresh remote dynamic cache, We force to clear the claims principal.
            context.ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var logger = context.GetRequiredService<ILogger<TContributor>>();
            logger.LogWarning(e, $"Failed to refresh remote dynamic claims cache for user: {userId.Value}");
            return;
        }

        if (dynamicClaims.Claims.IsNullOrEmpty())
        {
            return;
        }

        await AddDynamicClaimsAsync(context, identity, dynamicClaims.Claims);
    }
}
