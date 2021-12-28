using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer;

public class AbpClaimsService : DefaultClaimsService
{
    public AbpClaimsService(
        IProfileService profile,
        ILogger<DefaultClaimsService> logger)
        : base(profile, logger)
    {

    }

    protected override IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
    {
        var claimTypesArray = claimTypes.ToArray();
        return base.FilterRequestedClaimTypes(claimTypesArray).Union(FilterAdditionalRequestedClaimTypes(claimTypesArray));
    }

    protected virtual IEnumerable<string> FilterAdditionalRequestedClaimTypes(IEnumerable<string> claimTypes)
    {
        return new[] { AbpClaimTypes.TenantId };
    }

    protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
    {
        return base.GetOptionalClaims(subject).Union(GetAdditionalOptionalClaims(subject));
    }

    protected virtual IEnumerable<Claim> GetAdditionalOptionalClaims(ClaimsPrincipal subject)
    {
        var claim = subject.FindFirst(AbpClaimTypes.TenantId);
        if (claim != null)
        {
            yield return claim;
        }
    }
}
