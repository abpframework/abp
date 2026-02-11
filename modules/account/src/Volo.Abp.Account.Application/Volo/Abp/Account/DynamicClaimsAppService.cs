using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Account;

[Authorize]
public class DynamicClaimsAppService : IdentityAppServiceBase, IDynamicClaimsAppService
{
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; }
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }

    public DynamicClaimsAppService(
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
        ICurrentPrincipalAccessor principalAccessor)
    {
        IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;
        AbpClaimsPrincipalFactory = abpClaimsPrincipalFactory;
        PrincipalAccessor = principalAccessor;
    }

    public virtual async Task RefreshAsync()
    {
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(CurrentUser.GetId(), CurrentUser.TenantId);
        await AbpClaimsPrincipalFactory.CreateDynamicAsync(PrincipalAccessor.Principal);
    }
}
