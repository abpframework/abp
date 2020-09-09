using System;
using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity
{
    public static class CurrentPrincipalAccessorExtensions
    {
        public static IDisposable ChangeImpersonatorId(this ICurrentPrincipalAccessor currentPrincipalAccessor, Guid? tenantImpersonatorId, Guid? userImpersonatorId)
        {
            var impersonatorClaims = new List<Claim>();
            if (tenantImpersonatorId.HasValue)
            {
                impersonatorClaims.Add(new Claim(AbpClaimTypes.TenantImpersonatorId, tenantImpersonatorId.ToString()));
            }
            if (userImpersonatorId.HasValue)
            {
                impersonatorClaims.Add(new Claim(AbpClaimTypes.UserImpersonatorId, userImpersonatorId.ToString()));
            }

            return currentPrincipalAccessor.Change(impersonatorClaims);
        }
    }
}
