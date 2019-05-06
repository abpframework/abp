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

namespace Volo.Abp.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class IdentityModelAuthenticationService : IIdentityModelAuthenticationService, ITransientDependency
    {
        public ILogger<IdentityModelAuthenticationService> Logger { get; set; }
        protected IdentityClientOptions ClientOptions { get; }

        public IdentityModelAuthenticationService(
            IOptions<IdentityClientOptions> options)
        {
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

        protected virtual async Task<DiscoveryResponse> GetDiscoveryResponse(IdentityClientConfiguration configuration)
        {
            return await DiscoveryClient.GetAsync(configuration.Authority);
        }

        protected virtual async Task<TokenResponse> GetTokenResponse(DiscoveryResponse discoveryResponse, IdentityClientConfiguration configuration)
        {
            //TODO: Pass cancellation token

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
