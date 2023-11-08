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

        var dynamicClaims = principal.Claims
            .Where(c => AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims.Contains(c.Type))
            .Select(c => new DynamicClaimDto
            {
                Type = c.Type,
                Value = c.Value
            });

        return dynamicClaims.ToList();
    }
}
