using System.Security.Claims;
using Volo.Abp.AspNetCore.Components.UI.Security;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.UI
{
    public class ComponentsCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
    {
        protected AbpComponentsClaimsCache ClaimsCache { get; }

        public ComponentsCurrentPrincipalAccessor(
            AbpComponentsClaimsCache claimsCache)
        {
            ClaimsCache = claimsCache;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return ClaimsCache.Principal;
        }
    }
}
