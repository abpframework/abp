using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

        [CanBeNull]
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AbpComponentsClaimsCache(
            IClientScopeServiceProviderAccessor serviceProviderAccessor)
        {
            _authenticationStateProvider = serviceProviderAccessor.ServiceProvider.GetService<AuthenticationStateProvider>();
            if (_authenticationStateProvider != null)
            {
                _authenticationStateProvider.AuthenticationStateChanged += async (task) =>
                {
                    Principal = (await task).User;
                };
            }
        }

        public virtual async Task InitializeAsync()
        {
            if (_authenticationStateProvider != null)
            {
                var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                Principal = authenticationState.User;
            }
        }
    }
}
