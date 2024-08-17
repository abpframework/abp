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

    public async Task<AbpDynamicClaimCacheItem> GetAsync(Guid userId, Guid? tenantId = null)
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
            await RefreshAsync(userId, tenantId);
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, $"Failed to refresh remote claims for user: {userId}");
            throw;
        }

        dynamicClaims = await GetCacheAsync(userId, tenantId);
        if (dynamicClaims == null)
        {
            throw new AbpException($"Failed to refresh remote claims for user: {userId}");
        }

        return dynamicClaims;
    }

    protected abstract Task<AbpDynamicClaimCacheItem?> GetCacheAsync(Guid userId, Guid? tenantId = null);

    protected abstract Task RefreshAsync(Guid userId, Guid? tenantId = null);
}
