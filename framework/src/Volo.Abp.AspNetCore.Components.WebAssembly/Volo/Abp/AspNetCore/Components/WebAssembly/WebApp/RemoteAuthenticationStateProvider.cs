using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public class RemoteAuthenticationStateProvider : AuthenticationStateProvider
{
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    protected WebAssemblyCachedApplicationConfigurationClient WebAssemblyCachedApplicationConfigurationClient { get; }

    public RemoteAuthenticationStateProvider(
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        WebAssemblyCachedApplicationConfigurationClient webAssemblyCachedApplicationConfigurationClient)
    {
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        WebAssemblyCachedApplicationConfigurationClient = webAssemblyCachedApplicationConfigurationClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (CurrentPrincipalAccessor.Principal.Identity == null ||
            !CurrentPrincipalAccessor.Principal.Identity.IsAuthenticated)
        {
            await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
        }

        return new AuthenticationState(CurrentPrincipalAccessor.Principal);
    }
}
