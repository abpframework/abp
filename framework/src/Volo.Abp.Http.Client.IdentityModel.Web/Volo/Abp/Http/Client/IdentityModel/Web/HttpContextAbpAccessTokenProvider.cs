using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Users;

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

        if (!httpContext.RequestServices.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            return null;
        }

        return await httpContext.GetTokenAsync("access_token");
    }
}
