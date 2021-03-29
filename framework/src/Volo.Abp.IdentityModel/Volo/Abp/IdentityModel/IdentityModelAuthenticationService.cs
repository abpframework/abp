using IdentityModel;
using IdentityModel.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class IdentityModelAuthenticationService : IIdentityModelAuthenticationService, ITransientDependency
    {
        public const string HttpClientName = "IdentityModelAuthenticationServiceHttpClientName";
        public ILogger<IdentityModelAuthenticationService> Logger { get; set; }
        protected AbpIdentityClientOptions ClientOptions { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }
        protected IHttpClientFactory HttpClientFactory { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IdentityModelHttpRequestMessageOptions IdentityModelHttpRequestMessageOptions { get; }
        protected IDistributedCache<IdentityModelTokenCacheItem> TokenCache { get; }
        protected IDistributedCache<IdentityModelDiscoveryDocumentCacheItem> DiscoveryDocumentCache { get; }

        public IdentityModelAuthenticationService(
            IOptions<AbpIdentityClientOptions> options,
            ICancellationTokenProvider cancellationTokenProvider,
            IHttpClientFactory httpClientFactory,
            ICurrentTenant currentTenant,
            IOptions<IdentityModelHttpRequestMessageOptions> identityModelHttpRequestMessageOptions,
            IDistributedCache<IdentityModelTokenCacheItem> tokenCache,
            IDistributedCache<IdentityModelDiscoveryDocumentCacheItem> discoveryDocumentCache)
        {
            ClientOptions = options.Value;
            CancellationTokenProvider = cancellationTokenProvider;
            HttpClientFactory = httpClientFactory;
            CurrentTenant = currentTenant;
            TokenCache = tokenCache;
            DiscoveryDocumentCache = discoveryDocumentCache;
            IdentityModelHttpRequestMessageOptions = identityModelHttpRequestMessageOptions.Value;
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
            var cacheKey = CalculateTokenCacheKey(configuration);
            var tokenCacheItem = await TokenCache.GetAsync(cacheKey);
            if (tokenCacheItem == null)
            {
                var tokenResponse = await GetTokenResponse(configuration);

                if (tokenResponse.IsError)
                {
                    if (tokenResponse.ErrorDescription != null)
                    {
                        throw new AbpException($"Could not get token from the OpenId Connect server! ErrorType: {tokenResponse.ErrorType}. " +
                                               $"Error: {tokenResponse.Error}. ErrorDescription: {tokenResponse.ErrorDescription}. HttpStatusCode: {tokenResponse.HttpStatusCode}");
                    }

                    var rawError = tokenResponse.Raw;
                    var withoutInnerException = rawError.Split(new string[] { "<eof/>" }, StringSplitOptions.RemoveEmptyEntries);
                    throw new AbpException(withoutInnerException[0]);
                }

                tokenCacheItem = new IdentityModelTokenCacheItem(tokenResponse.AccessToken);
                await TokenCache.SetAsync(cacheKey, tokenCacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.CacheAbsoluteExpiration)
                    });
            }

            return tokenCacheItem.AccessToken;
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

        protected virtual async Task<string> GetTokenEndpoint(IdentityClientConfiguration configuration)
        {
            //TODO: Can use (configuration.Authority + /connect/token) directly?

            var tokenEndpointUrlCacheKey = CalculateDiscoveryDocumentCacheKey(configuration);
            var discoveryDocumentCacheItem = await DiscoveryDocumentCache.GetAsync(tokenEndpointUrlCacheKey);
            if (discoveryDocumentCacheItem == null)
            {
                var discoveryResponse = await GetDiscoveryResponse(configuration);
                if (discoveryResponse.IsError)
                {
                    throw new AbpException($"Could not retrieve the OpenId Connect discovery document! " +
                                           $"ErrorType: {discoveryResponse.ErrorType}. Error: {discoveryResponse.Error}");
                }

                discoveryDocumentCacheItem = new IdentityModelDiscoveryDocumentCacheItem(discoveryResponse.TokenEndpoint);
                await DiscoveryDocumentCache.SetAsync(tokenEndpointUrlCacheKey, discoveryDocumentCacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(configuration.CacheAbsoluteExpiration)
                    });
            }

            return discoveryDocumentCacheItem.TokenEndpoint;
        }

        protected virtual async Task<DiscoveryDocumentResponse> GetDiscoveryResponse(IdentityClientConfiguration configuration)
        {
            using (var httpClient = HttpClientFactory.CreateClient(HttpClientName))
            {
                var request = new DiscoveryDocumentRequest
                {
                    Address = configuration.Authority,
                    Policy =
                    {
                        RequireHttps = configuration.RequireHttps
                    }
                };
                IdentityModelHttpRequestMessageOptions.ConfigureHttpRequestMessage?.Invoke(request);
                return await httpClient.GetDiscoveryDocumentAsync(request);
            }
        }

        protected virtual async Task<TokenResponse> GetTokenResponse(IdentityClientConfiguration configuration)
        {
            var tokenEndpoint = await GetTokenEndpoint(configuration);

            using (var httpClient = HttpClientFactory.CreateClient(HttpClientName))
            {
                AddHeaders(httpClient);

                switch (configuration.GrantType)
                {
                    case OidcConstants.GrantTypes.ClientCredentials:
                        return await httpClient.RequestClientCredentialsTokenAsync(
                            await CreateClientCredentialsTokenRequestAsync(tokenEndpoint, configuration),
                            CancellationTokenProvider.Token
                        );
                    case OidcConstants.GrantTypes.Password:
                        return await httpClient.RequestPasswordTokenAsync(
                            await CreatePasswordTokenRequestAsync(tokenEndpoint, configuration),
                            CancellationTokenProvider.Token
                        );
                    default:
                        throw new AbpException("Grant type was not implemented: " + configuration.GrantType);
                }
            }
        }

        protected virtual Task<PasswordTokenRequest> CreatePasswordTokenRequestAsync(string tokenEndpoint, IdentityClientConfiguration configuration)
        {
            var request = new PasswordTokenRequest
            {
                Address = tokenEndpoint,
                Scope = configuration.Scope,
                ClientId = configuration.ClientId,
                ClientSecret = configuration.ClientSecret,
                UserName = configuration.UserName,
                Password = configuration.UserPassword
            };
            IdentityModelHttpRequestMessageOptions.ConfigureHttpRequestMessage?.Invoke(request);

            AddParametersToRequestAsync(configuration, request);

            return Task.FromResult(request);
        }

        protected virtual Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequestAsync(string tokenEndpoint, IdentityClientConfiguration configuration)
        {
            var request = new ClientCredentialsTokenRequest
            {
                Address = tokenEndpoint,
                Scope = configuration.Scope,
                ClientId = configuration.ClientId,
                ClientSecret = configuration.ClientSecret
            };
            IdentityModelHttpRequestMessageOptions.ConfigureHttpRequestMessage?.Invoke(request);

            AddParametersToRequestAsync(configuration, request);

            return Task.FromResult(request);
        }

        protected virtual Task AddParametersToRequestAsync(IdentityClientConfiguration configuration, ProtocolRequest request)
        {
            foreach (var pair in configuration.Where(p => p.Key.StartsWith("[o]", StringComparison.OrdinalIgnoreCase)))
            {
                request.Parameters.Add(pair);
            }

            return Task.CompletedTask;
        }

        protected virtual void AddHeaders(HttpClient client)
        {
            //tenantId
            if (CurrentTenant.Id.HasValue)
            {
                //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
                client.DefaultRequestHeaders.Add(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.Value.ToString());
            }
        }

        protected virtual string CalculateDiscoveryDocumentCacheKey(IdentityClientConfiguration configuration)
        {
            return IdentityModelDiscoveryDocumentCacheItem.CalculateCacheKey(configuration);
        }

        protected virtual string CalculateTokenCacheKey(IdentityClientConfiguration configuration)
        {
            return IdentityModelTokenCacheItem.CalculateCacheKey(configuration);
        }
    }
}
