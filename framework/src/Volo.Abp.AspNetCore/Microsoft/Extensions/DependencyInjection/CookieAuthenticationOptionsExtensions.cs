using System;
using System.Globalization;
using System.Threading.Tasks;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace Microsoft.Extensions.DependencyInjection;

public static class CookieAuthenticationOptionsExtensions
{
    /// <summary>
    /// Check if the access_token is expired or inactive.
    /// </summary>
    public static CookieAuthenticationOptions CheckTokenExpiration(this CookieAuthenticationOptions options, string oidcAuthenticationScheme = "oidc", TimeSpan? advance = null, TimeSpan? validationInterval = null)
    {
        advance ??= TimeSpan.FromMinutes(3);
        validationInterval ??= TimeSpan.FromMinutes(1);
        var previousHandler = options.Events.OnValidatePrincipal;
        options.Events.OnValidatePrincipal = async principalContext =>
        {
            if (principalContext.Principal == null || principalContext.Principal.Identity == null || !principalContext.Principal.Identity.IsAuthenticated)
            {
                await InvokePreviousHandlerAsync(principalContext, previousHandler);
                return;
            }

            var logger = principalContext.HttpContext.RequestServices.GetRequiredService<ILogger<CookieAuthenticationOptions>>();

            var tokenExpiresAt = principalContext.Properties.GetString(".Token.expires_at");
            if (!tokenExpiresAt.IsNullOrWhiteSpace() && DateTimeOffset.TryParseExact(tokenExpiresAt, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresAt) &&
                expiresAt <= DateTimeOffset.UtcNow.Add(advance.Value))
            {
                var refreshToken = principalContext.Properties.GetTokenValue("refresh_token");
                if (refreshToken.IsNullOrWhiteSpace())
                {
                    await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                    return;
                }

                logger.LogInformation("The access_token expires within {AdvanceSeconds}s but a refresh_token is available; attempting to refresh.", advance.Value.TotalSeconds);

                var openIdConnectOptions = await GetOpenIdConnectOptions(principalContext, oidcAuthenticationScheme);

                var tokenEndpoint = openIdConnectOptions.Configuration?.TokenEndpoint;
                if (tokenEndpoint.IsNullOrWhiteSpace() && !openIdConnectOptions.Authority.IsNullOrWhiteSpace())
                {
                    tokenEndpoint = openIdConnectOptions.Authority.EnsureEndsWith('/') + "connect/token";
                }

                if (tokenEndpoint.IsNullOrWhiteSpace())
                {
                    logger.LogWarning("No token endpoint configured. Skipping token refresh.");
                    await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                    return;
                }

                var clientId = principalContext.Properties.GetString("client_id");
                var clientSecret = principalContext.Properties.GetString("client_secret");

                var refreshRequest = new RefreshTokenRequest
                {
                    Address = tokenEndpoint,
                    ClientId = clientId ?? openIdConnectOptions.ClientId!,
                    ClientSecret = clientSecret ?? openIdConnectOptions.ClientSecret,
                    RefreshToken = refreshToken
                };

                var cancellationTokenProvider = principalContext.HttpContext.RequestServices.GetRequiredService<ICancellationTokenProvider>();

                const int RefreshTokenLockTimeoutSeconds = 3;
                const string RefreshTokenLockKeyFormat = "refresh_token_lock_{0}";

                var userKey =
                    principalContext.Principal?.FindFirst("sub")?.Value
                    ?? principalContext.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                    ?? "unknown";

                var lockKey = string.Format(CultureInfo.InvariantCulture, RefreshTokenLockKeyFormat, userKey);
                var lockTimeout = TimeSpan.FromSeconds(RefreshTokenLockTimeoutSeconds);

                var abpDistributedLock = principalContext.HttpContext.RequestServices.GetRequiredService<IAbpDistributedLock>();

                await using (var handle = await abpDistributedLock.TryAcquireAsync(lockKey, lockTimeout, cancellationTokenProvider.Token))
                {
                    if (handle != null)
                    {
                        var response = await openIdConnectOptions.Backchannel.RequestRefreshTokenAsync(refreshRequest, cancellationTokenProvider.Token);

                        if (response.IsError)
                        {
                            logger.LogError("Token refresh failed: {Error}", response.Error);
                            await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                            return;
                        }

                        if (response.ExpiresIn <= 0)
                        {
                            logger.LogWarning("The token endpoint response does not contain a valid expires_in value. Skipping token refresh.");
                            await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                            return;
                        }

                        if (response.AccessToken.IsNullOrWhiteSpace())
                        {
                            logger.LogWarning("The token endpoint response does not contain a new access_token. Skipping token refresh.");
                            await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                            return;
                        }

                        if (response.RefreshToken.IsNullOrWhiteSpace())
                        {
                            logger.LogInformation("The token endpoint response does not contain a new refresh_token. The old refresh_token will continue to be used until it expires.");
                        }

                        logger.LogInformation("Token refreshed successfully. Updating cookie with new tokens.");
                        var newTokens = new[]
                        {
                            new AuthenticationToken { Name = "access_token", Value = response.AccessToken },
                            new AuthenticationToken { Name = "refresh_token", Value = response.RefreshToken ?? refreshToken },
                            new AuthenticationToken { Name = "expires_at", Value = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn).ToString("o", CultureInfo.InvariantCulture) }
                        };

                        principalContext.Properties.StoreTokens(newTokens);
                        principalContext.ShouldRenew = true;

                        await InvokePreviousHandlerAsync(principalContext, previousHandler);
                        return;
                    }
                }

                logger.LogInformation("The access_token expires within {AdvanceSeconds}s; signing out.", advance.Value.TotalSeconds);
                await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                return;
            }

            if (principalContext.Properties.IssuedUtc != null && DateTimeOffset.UtcNow.Subtract(principalContext.Properties.IssuedUtc.Value) > validationInterval)
            {
                logger.LogInformation("Checking access_token activity every {Seconds} seconds.", validationInterval.Value.TotalSeconds);
                var accessToken = principalContext.Properties.GetTokenValue("access_token");
                if (!accessToken.IsNullOrWhiteSpace())
                {
                    var openIdConnectOptions = await GetOpenIdConnectOptions(principalContext, oidcAuthenticationScheme);

                    var introspectionEndpoint = openIdConnectOptions.Configuration?.IntrospectionEndpoint;
                    if (introspectionEndpoint.IsNullOrWhiteSpace() && !openIdConnectOptions.Authority.IsNullOrWhiteSpace())
                    {
                        introspectionEndpoint = openIdConnectOptions.Authority.EnsureEndsWith('/') + "connect/introspect";
                    }

                    if (introspectionEndpoint.IsNullOrWhiteSpace())
                    {
                        logger.LogWarning("No introspection endpoint configured. Skipping token activity check.");
                        await InvokePreviousHandlerAsync(principalContext, previousHandler);
                        return;
                    }

                    var clientId = principalContext.Properties.GetString("client_id");
                    var clientSecret = principalContext.Properties.GetString("client_secret");
                    var response = await openIdConnectOptions.Backchannel.IntrospectTokenAsync(new TokenIntrospectionRequest
                    {
                        Address = introspectionEndpoint,
                        ClientId = clientId ?? openIdConnectOptions.ClientId!,
                        ClientSecret = clientSecret ?? openIdConnectOptions.ClientSecret,
                        Token = accessToken
                    });

                    if (response.IsError)
                    {
                        logger.LogError("Token introspection error: {Error}", response.Error);
                        await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                        return;
                    }

                    if (!response.IsActive)
                    {
                        logger.LogError("The access_token is not active.");
                        await SignOutAndInvokePreviousHandlerAsync(principalContext, previousHandler);
                        return;
                    }

                    logger.LogInformation("The access_token is active.");
                    principalContext.ShouldRenew = true;
                }
                else
                {
                    logger.LogError("The access_token is not found in the cookie properties. Ensure SaveTokens of OpenIdConnectOptions is true.");
                    await SignOutAsync(principalContext);
                }
            }

            await InvokePreviousHandlerAsync(principalContext, previousHandler);
        };

        return options;
    }

    private static async Task<OpenIdConnectOptions> GetOpenIdConnectOptions(CookieValidatePrincipalContext principalContext, string oidcAuthenticationScheme)
    {
        var openIdConnectOptions = principalContext.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>().Get(oidcAuthenticationScheme);
        var cancellationTokenProvider = principalContext.HttpContext.RequestServices.GetRequiredService<ICancellationTokenProvider>();
        if (openIdConnectOptions.Configuration == null && openIdConnectOptions.ConfigurationManager != null)
        {
            openIdConnectOptions.Configuration = await openIdConnectOptions.ConfigurationManager.GetConfigurationAsync(cancellationTokenProvider.Token);
        }

        return openIdConnectOptions;
    }

    private static async Task SignOutAsync(CookieValidatePrincipalContext principalContext)
    {
        principalContext.RejectPrincipal();
        await principalContext.HttpContext.SignOutAsync(principalContext.Scheme.Name);
    }

    private static Task InvokePreviousHandlerAsync(CookieValidatePrincipalContext principalContext, Func<CookieValidatePrincipalContext, Task>? previousHandler)
    {
        return previousHandler != null ? previousHandler(principalContext) : Task.CompletedTask;
    }

    private static async Task SignOutAndInvokePreviousHandlerAsync(CookieValidatePrincipalContext principalContext, Func<CookieValidatePrincipalContext, Task>? previousHandler)
    {
        await SignOutAsync(principalContext);
        await InvokePreviousHandlerAsync(principalContext, previousHandler);
    }
}
