using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteDynamicClaimsPrincipalContributorCache : ITransientDependency
{
    public const string HttpClientName = nameof(RemoteDynamicClaimsPrincipalContributorCache);

    public ILogger<RemoteDynamicClaimsPrincipalContributorCache> Logger { get; set; }
    protected IDistributedCache<AbpDynamicClaimCacheItem> Cache { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }
    protected IRemoteServiceHttpClientAuthenticator HttpClientAuthenticator { get; }

    public RemoteDynamicClaimsPrincipalContributorCache(
        IDistributedCache<AbpDynamicClaimCacheItem> cache,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
        IRemoteServiceHttpClientAuthenticator httpClientAuthenticator)
    {
        Cache = cache;
        HttpClientFactory = httpClientFactory;
        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOptions;
        HttpClientAuthenticator = httpClientAuthenticator;

        Logger = NullLogger<RemoteDynamicClaimsPrincipalContributorCache>.Instance;
    }

    public virtual async Task<AbpDynamicClaimCacheItem> GetAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"Get dynamic claims cache for user: {userId}");
        var dynamicClaims = await Cache.GetAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
        if (dynamicClaims != null && !dynamicClaims.Claims.IsNullOrEmpty())
        {
            return dynamicClaims;
        }

        Logger.LogDebug($"Refresh dynamic claims for user: {userId} from remote service.");
        try
        {
            var client = HttpClientFactory.CreateClient(HttpClientName);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, AbpClaimsPrincipalFactoryOptions.Value.RemoteRefreshUrl);
            await HttpClientAuthenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, requestMessage, new RemoteServiceConfiguration("/"), string.Empty));
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, $"Failed to refresh remote claims for user: {userId}");
            throw;
        }

        dynamicClaims = await Cache.GetAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
        if (dynamicClaims == null)
        {
            throw new AbpException($"Failed to refresh remote claims for user: {userId}");
        }

        return dynamicClaims;
    }
}
