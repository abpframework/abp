using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class WebAssemblyAuthenticationStateProvider<TRemoteAuthenticationState, TAccount, TProviderOptions> : RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, TProviderOptions>
    where TRemoteAuthenticationState : RemoteAuthenticationState
    where TProviderOptions : new()
    where TAccount : RemoteUserAccount
{
    protected WebAssemblyCachedApplicationConfigurationClient WebAssemblyCachedApplicationConfigurationClient { get; }

    [Obsolete]
    public WebAssemblyAuthenticationStateProvider(
        IJSRuntime jsRuntime,
        IOptionsSnapshot<RemoteAuthenticationOptions<TProviderOptions>> options,
        NavigationManager navigation,
        AccountClaimsPrincipalFactory<TAccount> accountClaimsPrincipalFactory,
        ILogger<WebAssemblyAuthenticationStateProvider<TRemoteAuthenticationState, TAccount, TProviderOptions>> logger,
        WebAssemblyCachedApplicationConfigurationClient webAssemblyCachedApplicationConfigurationClient)
        : base(jsRuntime, options, navigation, accountClaimsPrincipalFactory)
    {
        WebAssemblyCachedApplicationConfigurationClient = webAssemblyCachedApplicationConfigurationClient;
    }

    public WebAssemblyAuthenticationStateProvider(
        IJSRuntime jsRuntime,
        IOptionsSnapshot<RemoteAuthenticationOptions<TProviderOptions>> options,
        NavigationManager navigation,
        AccountClaimsPrincipalFactory<TAccount> accountClaimsPrincipalFactory,
        ILogger<RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, TProviderOptions>>? logger,
        ILogger<WebAssemblyAuthenticationStateProvider<TRemoteAuthenticationState, TAccount, TProviderOptions>> logger1,
        WebAssemblyCachedApplicationConfigurationClient webAssemblyCachedApplicationConfigurationClient)
        : base(jsRuntime, options, navigation, accountClaimsPrincipalFactory, logger)
    {
        WebAssemblyCachedApplicationConfigurationClient = webAssemblyCachedApplicationConfigurationClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = await base.GetAuthenticationStateAsync();
        var applicationConfigurationDto = await WebAssemblyCachedApplicationConfigurationClient.GetAsync();
        if (state.User.Identity != null && state.User.Identity.IsAuthenticated && !applicationConfigurationDto.CurrentUser.IsAuthenticated)
        {
            await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
        }

        return state;
    }
}
