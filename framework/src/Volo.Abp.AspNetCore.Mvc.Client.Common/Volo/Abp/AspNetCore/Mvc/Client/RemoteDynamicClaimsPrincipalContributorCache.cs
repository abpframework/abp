using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteDynamicClaimsPrincipalContributorCache : RemoteDynamicClaimsPrincipalContributorCacheBase<RemoteDynamicClaimsPrincipalContributorCache>
{
    public const string HttpClientName = nameof(RemoteDynamicClaimsPrincipalContributorCache);

    protected IDistributedCache<AbpDynamicClaimCacheItem> Cache { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IRemoteServiceHttpClientAuthenticator HttpClientAuthenticator { get; }

    public RemoteDynamicClaimsPrincipalContributorCache(
        IDistributedCache<AbpDynamicClaimCacheItem> cache,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
        IRemoteServiceHttpClientAuthenticator httpClientAuthenticator)
        : base(abpClaimsPrincipalFactoryOptions)
    {
        Cache = cache;
        HttpClientFactory = httpClientFactory;
        HttpClientAuthenticator = httpClientAuthenticator;
    }

    protected async override Task<AbpDynamicClaimCacheItem?> GetCacheAsync(Guid userId, Guid? tenantId = null)
    {
        return await Cache.GetAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }

    protected async override Task RefreshAsync(Guid userId, Guid? tenantId = null)
    {
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
    }
}
