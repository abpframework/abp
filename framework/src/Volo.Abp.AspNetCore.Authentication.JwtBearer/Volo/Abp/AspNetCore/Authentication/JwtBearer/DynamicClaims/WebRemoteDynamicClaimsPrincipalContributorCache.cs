using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;

public class WebRemoteDynamicClaimsPrincipalContributorCache : RemoteDynamicClaimsPrincipalContributorCacheBase<WebRemoteDynamicClaimsPrincipalContributorCache>
{
    public const string HttpClientName = nameof(WebRemoteDynamicClaimsPrincipalContributorCache);

    protected IDistributedCache<AbpDynamicClaimCacheItem> Cache { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected IOptions<WebRemoteDynamicClaimsPrincipalContributorOptions> Options { get; }

    public WebRemoteDynamicClaimsPrincipalContributorCache(
        IDistributedCache<AbpDynamicClaimCacheItem> cache,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
        IHttpContextAccessor httpContextAccessor,
        IOptions<WebRemoteDynamicClaimsPrincipalContributorOptions> options)
        : base(abpClaimsPrincipalFactoryOptions)
    {
        Cache = cache;
        HttpClientFactory = httpClientFactory;
        HttpContextAccessor = httpContextAccessor;
        Options = options;
    }

    protected async override Task<AbpDynamicClaimCacheItem?> GetCacheAsync(Guid userId, Guid? tenantId = null)
    {
        return await Cache.GetAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }

    protected async override Task RefreshAsync(Guid userId, Guid? tenantId = null)
    {
        try
        {
            if (HttpContextAccessor.HttpContext == null)
            {
                throw new AbpException($"Failed to refresh remote claims for user: {userId} - HttpContext is null!");
            }

            var authenticateResult = await HttpContextAccessor.HttpContext.AuthenticateAsync(Options.Value.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                throw new AbpException($"Failed to refresh remote claims for user: {userId} - authentication failed!");
            }

            var accessToken = authenticateResult.Properties?.GetTokenValue("access_token");
            if (accessToken.IsNullOrWhiteSpace())
            {
                throw new AbpException($"Failed to refresh remote claims for user: {userId} - access_token is null or empty!");
            }

            var client = HttpClientFactory.CreateClient(HttpClientName);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, AbpClaimsPrincipalFactoryOptions.Value.RemoteRefreshUrl);
            requestMessage.SetBearerToken(accessToken);
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
