using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public class RemoteAuthenticationStateProvider : AuthenticationStateProvider
{
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    protected WebAssemblyCachedApplicationConfigurationClient WebAssemblyCachedApplicationConfigurationClient { get; }
    protected IServiceProvider ServiceProvider { get; }

    public RemoteAuthenticationStateProvider(
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        WebAssemblyCachedApplicationConfigurationClient webAssemblyCachedApplicationConfigurationClient,
        IServiceProvider serviceProvider)
    {
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        WebAssemblyCachedApplicationConfigurationClient = webAssemblyCachedApplicationConfigurationClient;
        ServiceProvider = serviceProvider;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (ServiceProvider.GetService<AddBlazorWebAppTieredServicesHasBeenCalled>() != null)
        {
            if (CurrentPrincipalAccessor.Principal.Identity == null || !CurrentPrincipalAccessor.Principal.Identity.IsAuthenticated)
            {
                await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
            }
        }

        return new AuthenticationState(CurrentPrincipalAccessor.Principal);
    }
}
