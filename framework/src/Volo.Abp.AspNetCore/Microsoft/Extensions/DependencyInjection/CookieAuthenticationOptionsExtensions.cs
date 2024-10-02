using System;
using System.Globalization;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Microsoft.Extensions.DependencyInjection;

public static class CookieAuthenticationOptionsExtensions
{
    /// <summary>
    /// Check the access_token is expired or inactive.
    /// </summary>
    public static CookieAuthenticationOptions CheckTokenExpiration(this CookieAuthenticationOptions options, string oidcAuthenticationScheme = "oidc", TimeSpan? advance = null, TimeSpan? validationInterval = null)
    {
        advance ??= TimeSpan.FromMinutes(3);
        validationInterval ??= TimeSpan.FromMinutes(1);
        options.Events.OnValidatePrincipal = async principalContext =>
        {
            if (principalContext.Principal == null || principalContext.Principal.Identity == null || !principalContext.Principal.Identity.IsAuthenticated)
            {
                return;
            }

            var logger = principalContext.HttpContext.RequestServices.GetRequiredService<ILogger<CookieAuthenticationOptions>>();

            var tokenExpiresAt = principalContext.Properties.GetString(".Token.expires_at");
            if (!tokenExpiresAt.IsNullOrWhiteSpace() && DateTimeOffset.TryParseExact(tokenExpiresAt, "o", null, DateTimeStyles.RoundtripKind, out var expiresAt) &&
                expiresAt < DateTimeOffset.UtcNow.Subtract(advance.Value))
            {
                logger.LogInformation("The access_token is expired.");
                await SignOutAsync(principalContext);
                return;
            }

            if (principalContext.Properties.IssuedUtc != null && DateTimeOffset.UtcNow.Subtract(principalContext.Properties.IssuedUtc.Value) > validationInterval)
            {
                logger.LogInformation($"Check the access_token is active every {validationInterval.Value.TotalSeconds} seconds.");
                var accessToken = principalContext.Properties.GetTokenValue("access_token");
                if (!accessToken.IsNullOrWhiteSpace())
                {
                    var openIdConnectOptions = await GetOpenIdConnectOptions(principalContext, oidcAuthenticationScheme);

                    var response = await openIdConnectOptions.Backchannel.IntrospectTokenAsync(new TokenIntrospectionRequest
                    {
                        Address = openIdConnectOptions.Configuration?.IntrospectionEndpoint ?? openIdConnectOptions.Authority!.EnsureEndsWith('/') + "connect/introspect",
                        ClientId = openIdConnectOptions.ClientId!,
                        ClientSecret = openIdConnectOptions.ClientSecret,
                        Token = accessToken
                    });

                    if (response.IsError)
                    {
                        logger.LogError(response.Error);
                        await SignOutAsync(principalContext);
                        return;
                    }

                    if (!response.IsActive)
                    {
                        logger.LogError("The access_token is not active.");
                        await SignOutAsync(principalContext);
                        return;
                    }

                    logger.LogInformation("The access_token is active.");
                    principalContext.ShouldRenew = true;
                }
                else
                {
                    logger.LogError("The access_token is not found in the cookie properties, Please make sure SaveTokens of OpenIdConnectOptions is set as true.");
                    await SignOutAsync(principalContext);
                }
            }
        };

        return options;
    }

    private async static Task<OpenIdConnectOptions> GetOpenIdConnectOptions(CookieValidatePrincipalContext principalContext, string oidcAuthenticationScheme)
    {
        var openIdConnectOptions = principalContext.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>().Get(oidcAuthenticationScheme);
        var cancellationTokenProvider = principalContext.HttpContext.RequestServices.GetRequiredService<ICancellationTokenProvider>();
        if (openIdConnectOptions.Configuration == null && openIdConnectOptions.ConfigurationManager != null)
        {
            openIdConnectOptions.Configuration = await openIdConnectOptions.ConfigurationManager.GetConfigurationAsync(cancellationTokenProvider.Token);
        }

        return openIdConnectOptions;
    }

    private async static Task SignOutAsync(CookieValidatePrincipalContext principalContext)
    {
        principalContext.RejectPrincipal();
        await principalContext.HttpContext.SignOutAsync(principalContext.Scheme.Name);
    }
}
