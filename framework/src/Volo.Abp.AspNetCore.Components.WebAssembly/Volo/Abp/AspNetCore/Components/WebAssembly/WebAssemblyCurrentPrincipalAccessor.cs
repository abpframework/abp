using System.Security.Claims;
using Volo.Abp.AspNetCore.Components.WebAssembly.Security;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
    {
        protected AbpWebAssemblyClaimsCache ClaimsCache { get; }

        public WebAssemblyCurrentPrincipalAccessor(
            AbpWebAssemblyClaimsCache claimsCache)
        {
            ClaimsCache = claimsCache;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return ClaimsCache.Principal;
        }
    }
}
