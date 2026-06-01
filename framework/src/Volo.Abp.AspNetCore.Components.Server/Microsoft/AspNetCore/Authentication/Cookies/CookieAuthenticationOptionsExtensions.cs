using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Authentication.Cookies;

public static class CookieAuthenticationOptionsExtensions
{
    /// <summary>
    /// Check the access_token is expired or inactive.
    /// </summary>
    [Obsolete("Use CheckTokenExpiration method instead.")]
    public static CookieAuthenticationOptions IntrospectAccessToken(this CookieAuthenticationOptions options, string oidcAuthenticationScheme = "oidc")
    {
        return options.CheckTokenExpiration(oidcAuthenticationScheme, null, TimeSpan.FromMinutes(1));
    }
}
