using System;
using System.Security.Claims;

namespace Volo.Abp.Security.Claims
{
    public interface ICurrentPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }

        IDisposable Change(ClaimsPrincipal principal);
    }
}
