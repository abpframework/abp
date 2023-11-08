using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Json;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteDynamicClaimsPrincipalContributorCache : ITransientDependency
{
    public const string HttpClientName = nameof(RemoteDynamicClaimsPrincipalContributorCache);

    public ILogger<RemoteDynamicClaimsPrincipalContributorCache> Logger { get; set; }
    protected IDistributedCache<List<AbpClaimCacheItem>> Cache { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IRemoteServiceHttpClientAuthenticator HttpClientAuthenticator { get; }
    protected IOptions<RemoteDynamicClaimsPrincipalContributorCacheOptions> CacheOptions { get; }

    public RemoteDynamicClaimsPrincipalContributorCache(
        IDistributedCache<List<AbpClaimCacheItem>> cache,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
        IJsonSerializer jsonSerializer,
        IRemoteServiceHttpClientAuthenticator httpClientAuthenticator,
        IOptions<RemoteDynamicClaimsPrincipalContributorCacheOptions> cacheOptions)
    {
        Cache = cache;
        HttpClientFactory = httpClientFactory;
        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOptions;
        JsonSerializer = jsonSerializer;
        HttpClientAuthenticator = httpClientAuthenticator;
        CacheOptions = cacheOptions;

        Logger = NullLogger<RemoteDynamicClaimsPrincipalContributorCache>.Instance;
    }

    public virtual async Task<List<AbpClaimCacheItem>> GetAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"Get dynamic claims cache for user: {userId}");
        //The UI may use the same cache as AuthServer in the tiered application.
        var claims = await Cache.GetAsync(AbpClaimCacheItem.CalculateCacheKey(userId, tenantId));
        if (!claims.IsNullOrEmpty())
        {
            return claims!;
        }

        Logger.LogDebug($"Get dynamic claims cache for user: {userId} from remote cache.");
        // Use independent cache for remote claims.
        return (await Cache.GetOrAddAsync($"{nameof(RemoteDynamicClaimsPrincipalContributorCache)}{AbpClaimCacheItem.CalculateCacheKey(userId, tenantId)}", async () =>
        {
            var dynamicClaims = new List<AbpClaimCacheItem>();
            Logger.LogDebug($"Get dynamic claims for user: {userId} from remote service.");
            try
            {
                var client = HttpClientFactory.CreateClient(HttpClientName);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, AbpClaimsPrincipalFactoryOptions.Value.RemoteUrl);
                await HttpClientAuthenticator.Authenticate(new RemoteServiceHttpClientAuthenticateContext(client, requestMessage, new RemoteServiceConfiguration("/"), string.Empty));
                var response = await client.SendAsync(requestMessage);
                dynamicClaims = JsonSerializer.Deserialize<List<AbpClaimCacheItem>>(await response.Content.ReadAsStringAsync());
                Logger.LogDebug($"Successfully got {dynamicClaims.Count} remote claims for user: {userId}");
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, $"Failed to get remote claims for user: {userId}");
            }
            return dynamicClaims;
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
        }))!;
    }

    public virtual async Task ClearAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"Clear dynamic claims cache for user: {userId}");
        Logger.LogDebug($"Clear dynamic claims cache from remote cache for user: {userId}");
        await Cache.RemoveAsync(AbpClaimCacheItem.CalculateCacheKey(userId, tenantId));
        await Cache.RemoveAsync($"{nameof(RemoteDynamicClaimsPrincipalContributorCache)}{AbpClaimCacheItem.CalculateCacheKey(userId, tenantId)}");
    }
}
