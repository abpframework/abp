using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

// Wraps the framework IAntiforgery so the antiforgery token's per-user identifier is computed against a
// normalized principal on every entry point (generation and validation, controllers and Razor Pages,
// ABP and built-in filters, cookie and bearer). This keeps the same user consistent across schemes whose
// user id claim carries a different issuer (e.g. "LOCAL AUTHORITY" for the Identity cookie vs. the token
// authority for a validated JWT or an OIDC cookie).
public class AbpAntiforgery : IAntiforgery
{
    protected IAntiforgery Inner { get; }

    protected AbpAntiForgeryOptions Options { get; }

    public AbpAntiforgery(
        IAntiforgery inner,
        IOptions<AbpAntiForgeryOptions> options)
    {
        Inner = inner;
        Options = options.Value;
    }

    public virtual AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
    {
        return WithNormalizedUser(httpContext, () => Inner.GetAndStoreTokens(httpContext));
    }

    public virtual AntiforgeryTokenSet GetTokens(HttpContext httpContext)
    {
        return WithNormalizedUser(httpContext, () => Inner.GetTokens(httpContext));
    }

    public virtual Task<bool> IsRequestValidAsync(HttpContext httpContext)
    {
        return WithNormalizedUserAsync(httpContext, () => Inner.IsRequestValidAsync(httpContext));
    }

    public virtual Task ValidateRequestAsync(HttpContext httpContext)
    {
        return WithNormalizedUserAsync(httpContext, async () =>
        {
            await Inner.ValidateRequestAsync(httpContext);
            return true;
        });
    }

    public virtual void SetCookieTokenAndHeader(HttpContext httpContext)
    {
        WithNormalizedUser(httpContext, () =>
        {
            Inner.SetCookieTokenAndHeader(httpContext);
            return true;
        });
    }

    protected virtual T WithNormalizedUser<T>(HttpContext httpContext, Func<T> action)
    {
        if (!Options.NormalizeUserIdClaimIssuer)
        {
            return action();
        }

        var normalizer = httpContext.RequestServices.GetRequiredService<IAbpAntiForgeryClaimsPrincipalNormalizer>();
        var authenticateResultFeature = httpContext.Features.Get<IAuthenticateResultFeature>();
        var originalResult = authenticateResultFeature?.AuthenticateResult;
        var originalPrincipal = httpContext.User;
        httpContext.User = normalizer.Normalize(originalPrincipal);
        try
        {
            return action();
        }
        finally
        {
            RestoreAuthenticationState(httpContext, authenticateResultFeature, originalResult, originalPrincipal);
        }
    }

    protected virtual async Task<T> WithNormalizedUserAsync<T>(HttpContext httpContext, Func<Task<T>> action)
    {
        if (!Options.NormalizeUserIdClaimIssuer)
        {
            return await action();
        }

        var normalizer = httpContext.RequestServices.GetRequiredService<IAbpAntiForgeryClaimsPrincipalNormalizer>();
        var authenticateResultFeature = httpContext.Features.Get<IAuthenticateResultFeature>();
        var originalResult = authenticateResultFeature?.AuthenticateResult;
        var originalPrincipal = httpContext.User;
        httpContext.User = normalizer.Normalize(originalPrincipal);
        try
        {
            return await action();
        }
        finally
        {
            RestoreAuthenticationState(httpContext, authenticateResultFeature, originalResult, originalPrincipal);
        }
    }

    protected virtual void RestoreAuthenticationState(
        HttpContext httpContext,
        IAuthenticateResultFeature? authenticateResultFeature,
        AuthenticateResult? originalResult,
        ClaimsPrincipal originalPrincipal)
    {
        // Assigning HttpContext.User drops the AuthenticateResult on the built-in feature, so restoring the
        // principal alone would leave downstream consumers (SignalR connection expiration, idle checks, ...)
        // without the authentication properties. IAuthenticateResultFeature does not require an implementation
        // to keep the two in sync, so restore both: the principal first, then the result.
        httpContext.User = originalPrincipal;

        if (originalResult != null && authenticateResultFeature != null)
        {
            authenticateResultFeature.AuthenticateResult = originalResult;
        }
    }
}
