using System;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        var originalHandler = options.Events.OnValidatePrincipal;
        options.Events.OnValidatePrincipal = async principalContext =>
        {
            originalHandler?.Invoke(principalContext);

            if (principalContext.Principal != null && principalContext.Principal.Identity != null && principalContext.Principal.Identity.IsAuthenticated)
            {
                var accessToken = principalContext.Properties.GetTokenValue("access_token");
                if (!accessToken.IsNullOrWhiteSpace())
                {
                    var openIdConnectOptions = principalContext.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>().Get(oidcAuthenticationScheme);
                    if (openIdConnectOptions.Configuration == null && openIdConnectOptions.ConfigurationManager != null)
                    {
                        openIdConnectOptions.Configuration = await openIdConnectOptions.ConfigurationManager.GetConfigurationAsync(principalContext.HttpContext.RequestAborted);
                    }

                    var response = await openIdConnectOptions.Backchannel.IntrospectTokenAsync(new TokenIntrospectionRequest
                    {
                        Address = openIdConnectOptions.Configuration?.IntrospectionEndpoint ?? openIdConnectOptions.Authority.EnsureEndsWith('/') + "connect/introspect",
                        ClientId = openIdConnectOptions.ClientId,
                        ClientSecret = openIdConnectOptions.ClientSecret,
                        Token = accessToken
                    });

                    if (response.IsActive)
                    {
                        return;
                    }
                }

                principalContext.RejectPrincipal();
                await principalContext.HttpContext.SignOutAsync(principalContext.Scheme.Name);
            }
        };

        return options;
    }
}
