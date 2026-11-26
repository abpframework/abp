using System.Threading.Tasks;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

[Dependency(ReplaceServices = true)]
public class HttpContextIdentityModelRemoteServiceHttpClientAuthenticator : IdentityModelRemoteServiceHttpClientAuthenticator
{
    protected IAbpAccessTokenProvider AccessTokenProvider { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }

    public HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(
        IIdentityModelAuthenticationService identityModelAuthenticationService,
        IAbpAccessTokenProvider accessTokenProvider)
        : this(identityModelAuthenticationService, accessTokenProvider, new HttpContextAccessor())
    {
    }

    public HttpContextIdentityModelRemoteServiceHttpClientAuthenticator(
        IIdentityModelAuthenticationService identityModelAuthenticationService,
        IAbpAccessTokenProvider accessTokenProvider,
        IHttpContextAccessor httpContextAccessor)
        : base(identityModelAuthenticationService)
    {
        AccessTokenProvider = accessTokenProvider;
        HttpContextAccessor = httpContextAccessor;
    }

    public async override Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
    {
        if (context.RemoteService.GetUseCurrentAccessToken() != false)
        {
            var accessToken = await AccessTokenProvider.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                context.Request.SetBearerToken(accessToken);
                return;
            }
        }

        if (HttpContextAccessor.HttpContext == null || context.RemoteService.GetAllowClientCredentialsInHttpContext())
        {
            await base.Authenticate(context);
        }
    }
}
