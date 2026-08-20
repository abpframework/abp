using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity.AspNetCore;

public static class AbpIdentitySessionCookieAuthenticationOptionsExtensions
{
    public static CookieAuthenticationOptions ValidateIdentitySession(this CookieAuthenticationOptions options)
    {
        var previousOnCheckSlidingExpiration = options.Events.OnCheckSlidingExpiration;
        options.Events.OnCheckSlidingExpiration = async cookieSlidingExpirationContext =>
        {
            await previousOnCheckSlidingExpiration(cookieSlidingExpirationContext);

            if (cookieSlidingExpirationContext.ShouldRenew &&
                !await IsIdentitySessionValidAsync(cookieSlidingExpirationContext.HttpContext, cookieSlidingExpirationContext.Principal))
            {
                cookieSlidingExpirationContext.ShouldRenew = false;
            }
        };

        var previousOnValidatePrincipal = options.Events.OnValidatePrincipal;
        options.Events.OnValidatePrincipal = async cookieValidatePrincipalContext =>
        {
            await previousOnValidatePrincipal(cookieValidatePrincipalContext);

            if (cookieValidatePrincipalContext.Principal != null &&
                !await IsIdentitySessionValidAsync(cookieValidatePrincipalContext.HttpContext, cookieValidatePrincipalContext.Principal))
            {
                cookieValidatePrincipalContext.ShouldRenew = false;
                cookieValidatePrincipalContext.RejectPrincipal();
            }
        };

        return options;
    }

    private static async Task<bool> IsIdentitySessionValidAsync(HttpContext httpContext, ClaimsPrincipal principal)
    {
        var sessionId = principal.FindSessionId();
        if (sessionId.IsNullOrWhiteSpace())
        {
            return true;
        }

        if (!httpContext.RequestServices.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value.IsDynamicClaimsEnabled)
        {
            return true;
        }

        var currentTenant = httpContext.RequestServices.GetRequiredService<ICurrentTenant>();
        var identitySessionChecker = httpContext.RequestServices.GetRequiredService<IIdentitySessionChecker>();
        using (currentTenant.Change(principal.FindTenantId()))
        {
            return await identitySessionChecker.IsValidateAsync(sessionId);
        }
    }
}
