using System.Security.Claims;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
    {
        public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}