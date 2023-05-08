using System;
using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Microsoft.Extensions.DependencyInjection;

public static class CookieAuthenticationOptionsExtensions
{
    public static CookieAuthenticationOptions CheckTokenExpiration(this CookieAuthenticationOptions options, TimeSpan? advance = null)
    {
        advance ??= TimeSpan.FromMinutes(5);
        var originalHandler = options.Events.OnValidatePrincipal;
        options.Events.OnValidatePrincipal = async principalContext =>
        {
            originalHandler?.Invoke(principalContext);
            if (principalContext.Principal != null && principalContext.Principal.Identity != null && principalContext.Principal.Identity.IsAuthenticated)
            {
                var tokenExpiresAt = principalContext.Properties.Items[".Token.expires_at"];
                if (tokenExpiresAt != null &&
                    DateTimeOffset.TryParseExact(tokenExpiresAt, "o", null,  DateTimeStyles.RoundtripKind, out var expiresAt) &&
                    expiresAt < DateTimeOffset.UtcNow.Subtract(advance.Value))
                {
                    principalContext.RejectPrincipal();
                    await principalContext.HttpContext.SignOutAsync(principalContext.Scheme.Name);
                }
            }
        };
        return options;
    }
}
