using IdentityModel;
using IdentityModel.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class IdentityModelAuthenticationService : IIdentityModelAuthenticationService, ITransientDependency
    {
        public ILogger<IdentityModelAuthenticationService> Logger { get; set; }
        protected IdentityClientOptions ClientOptions { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public IdentityModelAuthenticationService(
            IOptions<IdentityClientOptions> options,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            CancellationTokenProvider = cancellationTokenProvider;
            ClientOptions = options.Value;
            Logger = NullLogger<IdentityModelAuthenticationService>.Instance;
        }

        public async Task<bool> TryAuthenticateAsync(
            [NotNull] HttpClient client,
            string identityClientName = null)
        {
            var accessToken = await GetAccessTokenOrNullAsync(identityClientName);
            if (accessToken == null)
            {
                return false;
            }

            SetAccessToken(client, accessToken);
            return true;

        }

        protected virtual async Task<string> GetAccessTokenOrNullAsync(string identityClientName)
        {
            var configuration = GetClientConfiguration(identityClientName);
            if (configuration == null)
            {
                Logger.LogWarning($"Could not find {nameof(IdentityClientConfiguration)} for {identityClientName}. Either define a configuration for {identityClientName} or set a default configuration.");
                return null;
            }

            return await GetAccessTokenAsync(configuration);
        }

        public virtual async Task<string> GetAccessTokenAsync(IdentityClientConfiguration configuration)
        {
            var discoveryResponse = await GetDiscoveryResponse(configuration);
            if (discoveryResponse.IsError)
            {
                throw new AbpException($"Could not retrieve the OpenId Connect discovery document! ErrorType: {discoveryResponse.ErrorType}. Error: {discoveryResponse.Error}");
            }

            var tokenResponse = await GetTokenResponse(discoveryResponse, configuration);
            if (tokenResponse.IsError)
            {
                throw new AbpException($"Could not get token from the OpenId Connect server! ErrorType: {tokenResponse.ErrorType}. Error: {tokenResponse.Error}. ErrorDescription: {tokenResponse.ErrorDescription}. HttpStatusCode: {tokenResponse.HttpStatusCode}");
            }

            return tokenResponse.AccessToken;
        }

        protected virtual void SetAccessToken(HttpClient client, string accessToken)
        {
            //TODO: "Bearer" should be configurable
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private IdentityClientConfiguration GetClientConfiguration(string identityClientName = null)
        {
            if (identityClientName.IsNullOrEmpty())
            {
                return ClientOptions.IdentityClients.Default;
            }

            return ClientOptions.IdentityClients.GetOrDefault(identityClientName) ??
                   ClientOptions.IdentityClients.Default;
        }

        protected virtual async Task<DiscoveryResponse> GetDiscoveryResponse(
            IdentityClientConfiguration configuration)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = configuration.Authority,
                    Policy =
                    {
                        RequireHttps = configuration.RequireHttps
                    }
                });
            }
        }

        protected virtual async Task<TokenResponse> GetTokenResponse(
            DiscoveryResponse discoveryResponse, 
            IdentityClientConfiguration configuration)
        {
            using (var httpClient = new HttpClient())
            {
                switch (configuration.GrantType)
                {
                    case OidcConstants.GrantTypes.ClientCredentials:
                        return await httpClient.RequestClientCredentialsTokenAsync(
                            new ClientCredentialsTokenRequest
                            {
                                Address = discoveryResponse.TokenEndpoint,
                                Scope = configuration.Scope,
                                ClientId = configuration.ClientId,
                                ClientSecret = configuration.ClientSecret
                            },
                            CancellationTokenProvider.Token
                        );
                    case OidcConstants.GrantTypes.Password:
                        return await httpClient.RequestPasswordTokenAsync(
                            new PasswordTokenRequest
                            {
                                Address = discoveryResponse.TokenEndpoint,
                                Scope = configuration.Scope,
                                ClientId = configuration.ClientId,
                                ClientSecret = configuration.ClientSecret,
                                UserName = configuration.UserName,
                                Password = configuration.UserPassword
                            },
                            CancellationTokenProvider.Token
                        );
                    default:
                        throw new AbpException("Grant type was not implemented: " + configuration.GrantType);
                }
            }
        }
    }
}
