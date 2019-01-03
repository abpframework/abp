using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace NewFolder.Abp.Http.Client.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class IdentityModelHttpClientAuthenticator : IHttpClientAuthenticator, ITransientDependency
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public async Task Authenticate(HttpClientAuthenticateContext context)
        {
            var accessToken = await GetAccessTokenFromHttpContextOrNullAsync() ??
                              await GetAccessTokenFromServerOrNullAsync(context);

            if (accessToken != null)
            {
                //TODO: "Bearer" should be configurable
                context.Client.DefaultRequestHeaders.Authorization 
                    = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        protected virtual Task<string> GetAccessTokenFromServerOrNullAsync(HttpClientAuthenticateContext context)
        {
            return Task.FromResult((string) null);
        }

        protected virtual async Task<string> GetAccessTokenFromHttpContextOrNullAsync()
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
