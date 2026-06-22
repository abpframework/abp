using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public class AspNetCoreAbpAntiForgeryManager : IAbpAntiForgeryManager, ITransientDependency
{
    protected AbpAntiForgeryOptions Options { get; }

    protected HttpContext HttpContext => _httpContextAccessor.HttpContext!;

    private readonly IAntiforgery _antiforgery;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AspNetCoreAbpAntiForgeryManager(
        IAntiforgery antiforgery,
        IHttpContextAccessor httpContextAccessor,
        IOptions<AbpAntiForgeryOptions> options)
    {
        _antiforgery = antiforgery;
        _httpContextAccessor = httpContextAccessor;
        Options = options.Value;
    }

    public virtual void SetCookie()
    {
        HttpContext.Response.Cookies.Append(
            Options.TokenCookie.Name!,
            GenerateToken(),
            Options.TokenCookie.Build(HttpContext)
        );
    }

    public virtual string GenerateToken()
    {
        var httpContext = _httpContextAccessor.HttpContext!;

        if (!Options.NormalizeUserIdClaimIssuer)
        {
            return _antiforgery.GetAndStoreTokens(httpContext).RequestToken!;
        }

        var normalizer = httpContext.RequestServices.GetRequiredService<IAbpAntiForgeryClaimsPrincipalNormalizer>();
        var originalPrincipal = httpContext.User;
        httpContext.User = AsyncHelper.RunSync(() => normalizer.NormalizeAsync(originalPrincipal));
        try
        {
            return _antiforgery.GetAndStoreTokens(httpContext).RequestToken!;
        }
        finally
        {
            httpContext.User = originalPrincipal;
        }
    }
}
