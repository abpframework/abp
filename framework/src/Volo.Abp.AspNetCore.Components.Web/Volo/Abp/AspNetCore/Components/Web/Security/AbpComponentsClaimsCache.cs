using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Security
{
    [ExposeServices(
        typeof(AbpComponentsClaimsCache),
        typeof(IAsyncInitialize)
    )]
    public class AbpComponentsClaimsCache : IScopedDependency, IAsyncInitialize
    {
        public ClaimsPrincipal Principal { get; private set; }

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AbpComponentsClaimsCache(IClientScopeServiceProviderAccessor clientScopeServiceProviderAccessor)
        {
            _authenticationStateProvider = clientScopeServiceProviderAccessor.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
            _authenticationStateProvider.AuthenticationStateChanged += async (task) =>
            {
                Principal = (await task).User;
            };
        }

        public virtual async Task InitializeAsync()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            Principal = authenticationState.User;
        }
    }
}
