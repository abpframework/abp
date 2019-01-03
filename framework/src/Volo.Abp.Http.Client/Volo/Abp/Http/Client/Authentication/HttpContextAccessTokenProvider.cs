using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.Http.Client.Authentication
{
    public class HttpContextAccessTokenProvider : IAccessTokenProvider, ISingletonDependency
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public async Task<string> GetOrNullAsync(DynamicHttpClientProxyConfig config)
        {
            var httpContext = HttpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            return await httpContext.GetTokenAsync("access_token");
        }
    }
}