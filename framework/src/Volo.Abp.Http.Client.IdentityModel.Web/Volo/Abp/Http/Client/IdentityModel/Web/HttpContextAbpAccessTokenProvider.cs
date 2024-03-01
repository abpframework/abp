using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

[Dependency(ReplaceServices = true)]
public class HttpContextAbpAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected IOptions<AbpHttpClientIdentityModelWebOptions> Options { get; }

    public HttpContextAbpAccessTokenProvider(IHttpContextAccessor httpContextAccessor, IOptions<AbpHttpClientIdentityModelWebOptions> options)
    {
        HttpContextAccessor = httpContextAccessor;
        Options = options;
    }

    public virtual async Task<string?> GetTokenAsync()
    {
        var httpContext = HttpContextAccessor?.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        var accessToken = await httpContext.GetTokenAsync("access_token");
        if (!accessToken.IsNullOrEmpty())
        {
            return accessToken;
        }

        if (Options.Value.GetAccessTokenFromRequest)
        {
            accessToken = await Options.Value.RequestAccessTokenProvider(httpContext);
        }

        return accessToken;
    }
}
