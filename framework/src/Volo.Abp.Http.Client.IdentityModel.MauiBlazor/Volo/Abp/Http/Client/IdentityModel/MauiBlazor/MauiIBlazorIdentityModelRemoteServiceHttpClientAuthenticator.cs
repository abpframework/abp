using System.Threading.Tasks;
using IdentityModel.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;

namespace Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

[Dependency(ReplaceServices = true)]
public class MauiBlazorIdentityModelRemoteServiceHttpClientAuthenticator : IdentityModelRemoteServiceHttpClientAuthenticator
{
    protected IAbpAccessTokenProvider AccessTokenProvider { get; }

    public MauiBlazorIdentityModelRemoteServiceHttpClientAuthenticator(
        IIdentityModelAuthenticationService identityModelAuthenticationService,
        IAbpAccessTokenProvider abpAccessTokenProvider)
        : base(identityModelAuthenticationService)
    {
        AccessTokenProvider = abpAccessTokenProvider;
    }

    public async override Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
    {
        if (context.RemoteService.GetUseCurrentAccessToken() != false)
        {
            var accessToken = await AccessTokenProvider.GetTokenAsync();
            if (accessToken != null)
            {
                context.Request.SetBearerToken(accessToken);
                return;
            }
        }

        await base.Authenticate(context);
    }
}
