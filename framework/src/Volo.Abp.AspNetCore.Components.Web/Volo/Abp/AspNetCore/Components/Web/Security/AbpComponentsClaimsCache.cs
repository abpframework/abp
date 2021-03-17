using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Security
{
    [ExposeServices(
        typeof(AbpComponentsClaimsCache),
        typeof(IAsyncInitialize)
    )]
    public class AbpComponentsClaimsCache : ISingletonDependency, IAsyncInitialize
    {
        public ClaimsPrincipal Principal { get; private set; }

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AbpComponentsClaimsCache(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public virtual async Task InitializeAsync()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            Principal = authenticationState.User;
        }
    }
}
