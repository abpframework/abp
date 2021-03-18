using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.IdentityModel;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    [Dependency(ReplaceServices = true)]
    public class AccessTokenProviderIdentityModelRemoteServiceHttpClientAuthenticator : IdentityModelRemoteServiceHttpClientAuthenticator
    {
        protected IAccessTokenProvider AccessTokenProvider { get; }

        public AccessTokenProviderIdentityModelRemoteServiceHttpClientAuthenticator(
            IIdentityModelAuthenticationService identityModelAuthenticationService,
            IAccessTokenProvider accessTokenProvider)
            : base(identityModelAuthenticationService)
        {
            AccessTokenProvider = accessTokenProvider;
        }

        public override async Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
        {
            if (context.RemoteService.GetUseCurrentAccessToken() != false)
            {
                var accessToken = await GetAccessTokenFromAccessTokenProviderOrNullAsync();
                if (accessToken != null)
                {
                    context.Request.SetBearerToken(accessToken);
                    return;
                }
            }

            await base.Authenticate(context);
        }

        protected virtual async Task<string> GetAccessTokenFromAccessTokenProviderOrNullAsync()
        {
            var result = await AccessTokenProvider.RequestAccessToken();
            if (result.Status != AccessTokenResultStatus.Success)
            {
                return null;
            }

            result.TryGetToken(out var token);
            return token.Value;
        }
    }
}
