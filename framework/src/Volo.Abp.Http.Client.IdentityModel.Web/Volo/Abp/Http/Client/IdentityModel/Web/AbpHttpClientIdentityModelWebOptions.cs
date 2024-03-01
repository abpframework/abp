using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

public class AbpHttpClientIdentityModelWebOptions
{
    /// <summary>
    /// It can be set to true to get the access token from the request in microservices scenarios.
    /// </summary>
    public bool GetAccessTokenFromRequest { get; set; }

    /// <summary>
    /// The default implementation gets the access token from the Authorization header.
    /// You can set this property to a custom function to get the access token from the request.
    /// </summary>
    public Func<HttpContext, Task<string?>> RequestAccessTokenProvider { get; set; }

    public AbpHttpClientIdentityModelWebOptions()
    {
        GetAccessTokenFromRequest = false;
        RequestAccessTokenProvider = httpContext =>
        {
            var authorization = httpContext.Request.Headers.Authorization.ToString();
            if (!authorization.IsNullOrEmpty() && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult<string?>(authorization.Substring("Bearer ".Length).Trim());
            }

            return Task.FromResult<string?>(null);
        };
    }
}
