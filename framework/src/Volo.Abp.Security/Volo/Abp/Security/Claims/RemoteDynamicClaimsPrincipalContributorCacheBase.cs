using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Security.Claims;

public abstract class RemoteDynamicClaimsPrincipalContributorCacheBase<TContributorCache>
{
    public ILogger<TContributorCache> Logger { get; set; }

    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }

    protected RemoteDynamicClaimsPrincipalContributorCacheBase(IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions)
    {
        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOptions;

        Logger = NullLogger<TContributorCache>.Instance;
    }

    public Task<AbpDynamicClaimCacheItem> GetAsync(Guid userId, Guid? tenantId = null)
    {
        return GetAsync(userId, tenantId, () => RefreshAsync(userId, tenantId));
    }

    protected virtual async Task<AbpDynamicClaimCacheItem> GetAsync(Guid userId, Guid? tenantId, Func<Task> refresh)
    {
        Logger.LogDebug($"Get dynamic claims cache for user: {userId}");
        var dynamicClaims = await GetCacheAsync(userId, tenantId);
        if (dynamicClaims != null)
        {
            return dynamicClaims;
        }

        Logger.LogDebug($"Refresh dynamic claims for user: {userId} from remote service.");
        try
        {
            await refresh();
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, $"Failed to refresh remote claims for user: {userId}");
            throw;
        }

        dynamicClaims = await GetCacheAsync(userId, tenantId);
        if (dynamicClaims == null)
        {
            throw new AbpException($"Failed to get dynamic claims for user: {userId} from cache after refreshing, " +
                                   $"Ensure all applications use the same distributed cache and the same cache key prefix.");
        }

        return dynamicClaims;
    }

    /// <summary>
    /// Whether the session was revoked (for example by a logout). The dynamic claims are cached per user,
    /// so a cached user doesn't mean the session of the current token is still valid.
    /// </summary>
    public virtual Task<bool> IsSessionRevokedAsync(string sessionId)
    {
        return Task.FromResult(false);
    }

    protected abstract Task<AbpDynamicClaimCacheItem?> GetCacheAsync(Guid userId, Guid? tenantId = null);

    protected abstract Task RefreshAsync(Guid userId, Guid? tenantId = null);
}
