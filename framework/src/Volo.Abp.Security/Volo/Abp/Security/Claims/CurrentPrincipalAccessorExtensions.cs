using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Volo.Abp.Security.Claims
{
    public static class CurrentPrincipalAccessorExtensions
    {
        public static IDisposable Change(this ICurrentPrincipalAccessor currentPrincipalAccessor, Claim claim)
        {
            return currentPrincipalAccessor.Change(new[] {claim});
        }

        public static IDisposable Change(this ICurrentPrincipalAccessor currentPrincipalAccessor, IEnumerable<Claim> claims)
        {
            return currentPrincipalAccessor.Change(new ClaimsIdentity(claims));
        }

        public static IDisposable Change(this ICurrentPrincipalAccessor currentPrincipalAccessor, ClaimsIdentity claimsIdentity)
        {
            return currentPrincipalAccessor.Change(new ClaimsPrincipal(claimsIdentity));
        }
    }
}
