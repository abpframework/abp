using System.Security.Claims;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims;

public class ThreadCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ISingletonDependency
{
    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        var principal = Thread.CurrentPrincipal;
        if (principal == null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        return principal as ClaimsPrincipal ?? new ClaimsPrincipal(principal);
    }
}
