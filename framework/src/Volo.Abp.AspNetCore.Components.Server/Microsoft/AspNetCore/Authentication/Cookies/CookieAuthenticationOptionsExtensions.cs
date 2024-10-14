using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Microsoft.AspNetCore.Authentication.Cookies;

public static class CookieAuthenticationOptionsExtensions
{
    /// <summary>
    /// Introspect access token on validating the principal.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="oidcAuthenticationScheme"></param>
    /// <returns></returns>
    public static CookieAuthenticationOptions IntrospectAccessToken(this CookieAuthenticationOptions options, string oidcAuthenticationScheme = "oidc")
    {
        options.Events.OnValidatePrincipal = async principalContext =>
        {
            if (principalContext.Principal == null || principalContext.Principal.Identity == null || !principalContext.Principal.Identity.IsAuthenticated)
            {
                return;
            }

            var logger = principalContext.HttpContext.RequestServices.GetRequiredService<ILogger<CookieAuthenticationOptions>>();

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
            }
            else
            {
                logger.LogError("The access_token is not found in the cookie properties, Please make sure SaveTokens of OpenIdConnectOptions is set as true.");
                await SignOutAsync(principalContext);
            }
        };

        return options;
    }

    private async static Task<OpenIdConnectOptions> GetOpenIdConnectOptions(CookieValidatePrincipalContext principalContext, string oidcAuthenticationScheme)
    {
        var openIdConnectOptions = principalContext.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>().Get(oidcAuthenticationScheme);
        if (openIdConnectOptions.Configuration == null && openIdConnectOptions.ConfigurationManager != null)
        {
            var cancellationTokenProvider = principalContext.HttpContext.RequestServices.GetRequiredService<ICancellationTokenProvider>();
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
