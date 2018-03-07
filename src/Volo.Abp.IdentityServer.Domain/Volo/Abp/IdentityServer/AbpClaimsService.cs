using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer
{
    public class AbpClaimsService : DefaultClaimsService
    {
        public AbpClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger)
            : base(profile, logger)
        {
        }

        protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
        {
            var tenantClaim = subject.FindFirst(AbpClaimTypes.TenantId);
            if (tenantClaim == null)
            {
                return base.GetOptionalClaims(subject);
            }
            else
            {
                return base.GetOptionalClaims(subject).Union(new[] { tenantClaim });
            }
        }
    }
}
