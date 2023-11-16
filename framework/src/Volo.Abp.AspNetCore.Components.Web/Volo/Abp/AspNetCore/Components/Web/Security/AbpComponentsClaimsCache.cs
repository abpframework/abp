using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.Web.Security;

public class AbpComponentsClaimsCache : IScopedDependency
{
    public ClaimsPrincipal Principal { get; private set; } = default!;

    private readonly AuthenticationStateProvider? _authenticationStateProvider;
    private readonly IAbpClaimsPrincipalFactory _abpClaimsPrincipalFactory;

    public AbpComponentsClaimsCache(
        IClientScopeServiceProviderAccessor serviceProviderAccessor)
    {
        _authenticationStateProvider = serviceProviderAccessor.ServiceProvider.GetService<AuthenticationStateProvider>();
        _abpClaimsPrincipalFactory = serviceProviderAccessor.ServiceProvider.GetRequiredService<IAbpClaimsPrincipalFactory>();
        if (_authenticationStateProvider != null)
        {
            _authenticationStateProvider.AuthenticationStateChanged += async (task) =>
            {
                Principal = await _abpClaimsPrincipalFactory.CreateDynamicAsync((await task).User);
            };
        }
    }

    public virtual async Task InitializeAsync()
    {
        if (_authenticationStateProvider != null)
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            Principal = authenticationState.User;
            await _abpClaimsPrincipalFactory.CreateDynamicAsync(Principal);
        }
    }
}
