using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Http.Client.IdentityModel
{
    //TODO: This class should be optimized and improved:

    [Dependency(ReplaceServices = true)]
    public class IdentityModelHttpClientAuthenticator : IHttpClientAuthenticator, ITransientDependency
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        protected IdentityClientOptions ClientOptions { get; }

        public IdentityModelHttpClientAuthenticator(
            IOptions<IdentityClientOptions> options)
        {
            ClientOptions = options.Value;
        }

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

        protected virtual async Task<string> GetAccessTokenFromHttpContextOrNullAsync()
        {
            var httpContext = HttpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            return await httpContext.GetTokenAsync("access_token");
        }

        protected virtual async Task<string> GetAccessTokenFromServerOrNullAsync(HttpClientAuthenticateContext context)
        {
            var configuration = GetClientConfiguration(context);

            if (configuration == null)
            {
                return null;
            }

            var discoveryResponse = await GetDiscoveryResponse(configuration);
            if (discoveryResponse.IsError)
            {
                return null;
            }

            var tokenResponse = await GetTokenResponse(discoveryResponse, configuration);
            if (tokenResponse.IsError)
            {
                return null;
            }

            return tokenResponse.AccessToken;
        }

        private IdentityClientConfiguration GetClientConfiguration(HttpClientAuthenticateContext context)
        {
            var identityClientName = context.RemoteService.GetIdentityClient();
            if (identityClientName.IsNullOrEmpty())
            {
                return ClientOptions.IdentityClients.Default;
            }

            return ClientOptions.IdentityClients.GetOrDefault(identityClientName) ??
                   ClientOptions.IdentityClients.Default;
        }

        protected virtual async Task<DiscoveryResponse> GetDiscoveryResponse(IdentityClientConfiguration configuration)
        {
            return await DiscoveryClient.GetAsync(configuration.Authority);
        }

        protected virtual async Task<TokenResponse> GetTokenResponse(DiscoveryResponse discoveryResponse, IdentityClientConfiguration configuration)
        {
            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, configuration.ClientId, configuration.ClientSecret);

            switch (configuration.GrantType)
            {
                case OidcConstants.GrantTypes.ClientCredentials:
                    return await tokenClient.RequestClientCredentialsAsync(
                        configuration.Scope
                    );
                case OidcConstants.GrantTypes.Password:
                    return await tokenClient.RequestResourceOwnerPasswordAsync(
                        configuration.UserName,
                        configuration.UserPassword,
                        configuration.Scope
                    );
                default:
                    throw new AbpException("Grant type was not implemented: " + configuration.GrantType);
            }
        }
    }
}
