using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

/// <summary>
/// Blazor requests a new token each time it is initialized/refreshed.
/// This class is used to revoke a token that is no longer in use.
/// </summary>
public class WebAssemblyAuthenticationStateProvider<TRemoteAuthenticationState, TAccount, TProviderOptions> : RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, TProviderOptions>
    where TRemoteAuthenticationState : RemoteAuthenticationState
    where TProviderOptions : new()
    where TAccount : RemoteUserAccount
{
    protected ILogger<RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, TProviderOptions>> Logger { get; }
    protected WebAssemblyCachedApplicationConfigurationClient WebAssemblyCachedApplicationConfigurationClient { get; }
    protected IOptions<WebAssemblyAuthenticationStateProviderOptions> WebAssemblyAuthenticationStateProviderOptions { get; }
    protected IHttpClientFactory HttpClientFactory { get; }

    protected readonly static ConcurrentDictionary<string, string> AccessTokens = new ConcurrentDictionary<string, string>();

    public WebAssemblyAuthenticationStateProvider(
        IJSRuntime jsRuntime,
        IOptionsSnapshot<RemoteAuthenticationOptions<TProviderOptions>> options,
        NavigationManager navigation,
        AccountClaimsPrincipalFactory<TAccount> accountClaimsPrincipalFactory,
        ILogger<RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, TProviderOptions>>? logger,
        WebAssemblyCachedApplicationConfigurationClient webAssemblyCachedApplicationConfigurationClient,
        IOptions<WebAssemblyAuthenticationStateProviderOptions> webAssemblyAuthenticationStateProviderOptions,
        IHttpClientFactory httpClientFactory)
        : base(jsRuntime, options, navigation, accountClaimsPrincipalFactory, logger)
    {
        Logger = logger ?? NullLogger<RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, TProviderOptions>>.Instance;

        WebAssemblyCachedApplicationConfigurationClient = webAssemblyCachedApplicationConfigurationClient;
        WebAssemblyAuthenticationStateProviderOptions = webAssemblyAuthenticationStateProviderOptions;
        HttpClientFactory = httpClientFactory;

        AuthenticationStateChanged += async state =>
        {
            var user = await state;
            if (user.User.Identity == null || !user.User.Identity.IsAuthenticated)
            {
                return;
            }

            var accessToken = await FindAccessTokenAsync();
            if (!accessToken.IsNullOrWhiteSpace())
            {
                AccessTokens.TryAdd(accessToken, accessToken);
            }

            await TryRevokeOldAccessTokensAsync();
        };
    }

    protected async override ValueTask<ClaimsPrincipal> GetAuthenticatedUser()
    {
        var accessToken = await FindAccessTokenAsync();
        if (!accessToken.IsNullOrWhiteSpace())
        {
            AccessTokens.TryAdd(accessToken, accessToken);
        }

        await TryRevokeOldAccessTokensAsync();

        return await base.GetAuthenticatedUser();
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var accessToken = await FindAccessTokenAsync();
        if (!accessToken.IsNullOrWhiteSpace())
        {
            AccessTokens.TryAdd(accessToken, accessToken);
        }

        await TryRevokeOldAccessTokensAsync();

        var state = await base.GetAuthenticationStateAsync();
        var applicationConfigurationDto = await WebAssemblyCachedApplicationConfigurationClient.GetAsync();
        if (state.User.Identity != null && state.User.Identity.IsAuthenticated && !applicationConfigurationDto.CurrentUser.IsAuthenticated)
        {
            await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
        }

        return state;
    }

    protected virtual async Task<string?> FindAccessTokenAsync()
    {
        var result = await RequestAccessToken();
        if (result.Status != AccessTokenResultStatus.Success)
        {
            return null;
        }

        result.TryGetToken(out var token);
        return token?.Value;
    }

    protected virtual async Task TryRevokeOldAccessTokensAsync()
    {
        if (AccessTokens.Count <= 1)
        {
            return;
        }

        var oidcProviderOptions = Options.ProviderOptions?.As<OidcProviderOptions>();
        var authority = oidcProviderOptions?.Authority;
        var clientId = oidcProviderOptions?.ClientId;

        if (authority.IsNullOrWhiteSpace() || clientId.IsNullOrWhiteSpace())
        {
            return;
        }

        var revoked = false;
        var revokeAccessTokens = AccessTokens.Select(x => x.Value);
        var currentAccessToken = await FindAccessTokenAsync();
        foreach (var accessToken in revokeAccessTokens)
        {
            if (accessToken == currentAccessToken)
            {
                continue;
            }

            if (!accessToken.IsNullOrWhiteSpace() && !currentAccessToken.IsNullOrWhiteSpace())
            {
                var handler = new JwtSecurityTokenHandler();
                var currentSessionId = handler.ReadJwtToken(currentAccessToken)?.Claims?.FirstOrDefault(x => x.Type == AbpClaimTypes.SessionId);
                var sessionId = handler.ReadJwtToken(accessToken)?.Claims?.FirstOrDefault(x => x.Type == AbpClaimTypes.SessionId);
                if (sessionId?.Value == currentSessionId?.Value)
                {
                    continue;
                }
            }

            var httpClient = HttpClientFactory.CreateClient(nameof(WebAssemblyAuthenticationStateProvider<TRemoteAuthenticationState, TAccount, TProviderOptions>));
            var result = await httpClient.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = authority.EnsureEndsWith('/') + WebAssemblyAuthenticationStateProviderOptions.Value.TokenRevocationUrl,
                ClientId = clientId,
                Token = accessToken,
            });

            if (!result.IsError)
            {
                AccessTokens.TryRemove(accessToken, out _);
                revoked = true;
            }
            else
            {
                Logger.LogError(result.Raw);
            }
        }

        if (revoked)
        {
            await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
        }
    }
}

internal class OidcUser
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}
