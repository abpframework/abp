using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.Authentication
{
    public class HttpContextAccessTokenProvider : IAccessTokenProvider, ISingletonDependency
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public async Task<string> GetOrNullAsync()
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