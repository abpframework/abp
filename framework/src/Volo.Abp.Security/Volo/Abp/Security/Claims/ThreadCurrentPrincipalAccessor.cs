using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ISingletonDependency
    {
        public ThreadCurrentPrincipalAccessor(ILogger<ThreadCurrentPrincipalAccessor> logger) : base(logger)
        {

        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            Logger.LogInformation($"托管线程Id:{Thread.CurrentThread.ManagedThreadId}");
            return Thread.CurrentPrincipal as ClaimsPrincipal;
        }
    }
}
