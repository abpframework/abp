using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

[Dependency(ReplaceServices = true)]
public class HttpContextAbpAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    protected IHttpContextAccessor HttpContextAccessor { get; }

    public HttpContextAbpAccessTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public virtual async Task<string?> GetTokenAsync()
    {
        var httpContext = HttpContextAccessor?.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        return await httpContext.GetTokenAsync("access_token");
    }
}
