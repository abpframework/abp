using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Security
{
    [ExposeServices(
        typeof(AbpWebAssemblyClaimsCache),
        typeof(IAsyncInitialize)
    )]
    public class AbpWebAssemblyClaimsCache : ISingletonDependency, IAsyncInitialize
    {
        public ClaimsPrincipal Principal { get; private set; }

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AbpWebAssemblyClaimsCache(AuthenticationStateProvider authenticationStateProvider)
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
