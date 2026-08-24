using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public static class AbpRemoteDynamicClaimsCookieAuthenticationOptionsExtensions
{
    public static CookieAuthenticationOptions ValidateRemoteDynamicClaims(this CookieAuthenticationOptions options)
    {
        var previousOnCheckSlidingExpiration = options.Events.OnCheckSlidingExpiration;
        options.Events.OnCheckSlidingExpiration = async cookieSlidingExpirationContext =>
        {
            await previousOnCheckSlidingExpiration(cookieSlidingExpirationContext);

            if (cookieSlidingExpirationContext.ShouldRenew &&
                cookieSlidingExpirationContext.Principal != null &&
                !await AreRemoteDynamicClaimsValidAsync(cookieSlidingExpirationContext.HttpContext, cookieSlidingExpirationContext.Scheme.Name, cookieSlidingExpirationContext.Principal, cookieSlidingExpirationContext.Properties))
            {
                cookieSlidingExpirationContext.ShouldRenew = false;
            }
        };

        var previousOnValidatePrincipal = options.Events.OnValidatePrincipal;
        options.Events.OnValidatePrincipal = async cookieValidatePrincipalContext =>
        {
            await previousOnValidatePrincipal(cookieValidatePrincipalContext);

            if (cookieValidatePrincipalContext.Principal != null &&
                !await AreRemoteDynamicClaimsValidAsync(cookieValidatePrincipalContext.HttpContext, cookieValidatePrincipalContext.Scheme.Name, cookieValidatePrincipalContext.Principal, cookieValidatePrincipalContext.Properties))
            {
                cookieValidatePrincipalContext.ShouldRenew = false;
                cookieValidatePrincipalContext.RejectPrincipal();
            }
        };

        return options;
    }

    private static async Task<bool> AreRemoteDynamicClaimsValidAsync(HttpContext httpContext, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        if (scheme == IdentityConstants.ExternalScheme ||
            scheme == IdentityConstants.TwoFactorUserIdScheme ||
            scheme == IdentityConstants.TwoFactorRememberMeScheme)
        {
            return true;
        }

        var identity = principal.Identities.FirstOrDefault();
        var userId = identity?.FindUserId();
        if (userId == null)
        {
            return true;
        }

        var abpClaimsPrincipalFactoryOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value;
        if (!abpClaimsPrincipalFactoryOptions.IsDynamicClaimsEnabled || !abpClaimsPrincipalFactoryOptions.IsRemoteRefreshEnabled)
        {
            return true;
        }

        var accessToken = properties.GetTokenValue("access_token");
        if (accessToken.IsNullOrWhiteSpace())
        {
            return true;
        }

        var dynamicClaimsCache = httpContext.RequestServices.GetService<RemoteDynamicClaimsPrincipalContributorCache>();
        if (dynamicClaimsCache == null)
        {
            return true;
        }

        try
        {
            // The multi-tenancy middleware hasn't resolved the tenant yet, but the dynamic claims are cached per tenant.
            var tenantId = identity!.FindTenantId();
            using (httpContext.RequestServices.GetRequiredService<ICurrentTenant>().Change(tenantId))
            {
                await dynamicClaimsCache.GetAsync(userId.Value, tenantId, accessToken);
            }

            return true;
        }
        catch (Exception e)
        {
            httpContext.RequestServices
                .GetRequiredService<ILogger<AbpAspNetCoreMvcClientModule>>()
                .LogWarning(e, $"Failed to refresh remote dynamic claims for user: {userId.Value}, the authentication cookie is rejected.");
            return false;
        }
    }
}
