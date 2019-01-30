using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;

namespace Volo.Abp.Http.Client.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class IdentityModelRemoteServiceHttpClientAuthenticator : IRemoteServiceHttpClientAuthenticator, ITransientDependency
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        protected IIdentityModelHttpClientAuthenticator IdentityModelHttpClientAuthenticator { get; }

        public IdentityModelRemoteServiceHttpClientAuthenticator(IIdentityModelHttpClientAuthenticator identityModelHttpClientAuthenticator)
        {
            IdentityModelHttpClientAuthenticator = identityModelHttpClientAuthenticator;
        }

        public async Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
        {
            var accessToken = await GetAccessTokenFromHttpContextOrNullAsync();

            if (accessToken != null)
            {
                context.Client.SetBearerToken(accessToken);
            }
            else
            {
                await IdentityModelHttpClientAuthenticator.AuthenticateAsync(
                    new IdentityModelHttpClientAuthenticateContext(
                        context.Client,
                        context.RemoteService.GetIdentityClient()
                    )
                );
            }
        }

        protected virtual async Task<string> GetAccessTokenFromHttpContextOrNullAsync()
        {
            //TODO: What if the access_token in the current Http Request is not usable for this client?
            var httpContext = HttpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            return await httpContext.GetTokenAsync("access_token");
        }
    }
}
