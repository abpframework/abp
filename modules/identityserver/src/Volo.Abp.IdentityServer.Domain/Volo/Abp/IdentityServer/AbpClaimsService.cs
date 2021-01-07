using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer
{
    public class AbpClaimsService : DefaultClaimsService
    {
        private static readonly string[] AdditionalOptionalClaimNames =
        {
            AbpClaimTypes.TenantId,
            AbpClaimTypes.Name,
            AbpClaimTypes.SurName,
            JwtClaimTypes.PreferredUserName,
            JwtClaimTypes.GivenName,
            JwtClaimTypes.FamilyName,
        };

        public AbpClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger)
            : base(profile, logger)
        {
        }

        protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
        {
            return base.GetOptionalClaims(subject)
                .Union(GetAdditionalOptionalClaims(subject));
        }

        protected virtual IEnumerable<Claim> GetAdditionalOptionalClaims(ClaimsPrincipal subject)
        {
            foreach (var claimName in AdditionalOptionalClaimNames)
            {
                var claim = subject.FindFirst(claimName);
                if (claim != null)
                {
                    yield return claim;
                }
            }
        }
    }
}
