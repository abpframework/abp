using System;
using System.Linq;
using System.Security.Claims;

namespace Volo.Abp.Security.Claims
{
    public static class ClaimsIdentityExtensions
    {
        public static ClaimsIdentity AddIfNotContains(this ClaimsIdentity claimsIdentity, Claim claim)
        {
            if (!claimsIdentity.Claims.Any(existClaim =>
                existClaim != null &&
                string.Equals(existClaim.Type, claim.Type, StringComparison.OrdinalIgnoreCase)))
            {
                claimsIdentity.AddClaim(claim);
            }

            return claimsIdentity;
        }
    }
}
