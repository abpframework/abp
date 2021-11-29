using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
    {
        protected AbpComponentsClaimsCache ClaimsCache { get; }

        public WebAssemblyCurrentPrincipalAccessor(
            IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor)
        {
            ClaimsCache = clientScopeServiceProviderAccessor.ServiceProvider.GetRequiredService<AbpComponentsClaimsCache>();
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return ClaimsCache.Principal;
        }
    }
}
