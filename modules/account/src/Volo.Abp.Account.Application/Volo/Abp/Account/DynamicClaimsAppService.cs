using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Account;

[Authorize]
public class DynamicClaimsAppService : IdentityAppServiceBase, IDynamicClaimsAppService
{
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; }
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }

    public DynamicClaimsAppService(
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory,
        ICurrentPrincipalAccessor principalAccessor,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions)
    {
        AbpClaimsPrincipalFactory = abpClaimsPrincipalFactory;
        PrincipalAccessor = principalAccessor;
        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOptions;
    }

    public virtual async Task<List<DynamicClaimDto>> GetAsync()
    {
        var principal = await AbpClaimsPrincipalFactory.CreateAsync(PrincipalAccessor.Principal);

        var dynamicClaims = new List<DynamicClaimDto>();
        foreach (var claimType in AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims)
        {
            var claims = principal.Claims.Where(x => x.Type == claimType).ToList();
            if (claims.Any())
            {
                dynamicClaims.AddRange(claims.Select(claim => new DynamicClaimDto(claimType, claim.Value)));
            }
            else
            {
                dynamicClaims.Add(new DynamicClaimDto(claimType, null));
            }
        }

        return dynamicClaims;
    }
}
