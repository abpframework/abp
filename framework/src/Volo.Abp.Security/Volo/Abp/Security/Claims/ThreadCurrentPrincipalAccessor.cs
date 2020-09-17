using System.Security.Claims;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ISingletonDependency
    {
        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return Thread.CurrentPrincipal as ClaimsPrincipal;
        }
    }
}
