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

        protected override IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
        {
            return base.FilterRequestedClaimTypes(claimTypes).Union(new []{AbpClaimTypes.TenantId, AbpClaimTypes.EditionId});
        }
    }
}
