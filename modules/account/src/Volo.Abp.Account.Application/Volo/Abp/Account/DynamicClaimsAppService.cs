using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Account;

[Authorize]
public class DynamicClaimsAppService(
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
    IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
    ICurrentPrincipalAccessor principalAccessor) : IdentityAppServiceBase, IDynamicClaimsAppService
{
    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; } = identityDynamicClaimsPrincipalContributorCache;
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; } = abpClaimsPrincipalFactory;
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; } = principalAccessor;

    public virtual async Task RefreshAsync()
    {
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(CurrentUser.GetId(), CurrentUser.TenantId);
        await AbpClaimsPrincipalFactory.CreateDynamicAsync(PrincipalAccessor.Principal);
    }
}
