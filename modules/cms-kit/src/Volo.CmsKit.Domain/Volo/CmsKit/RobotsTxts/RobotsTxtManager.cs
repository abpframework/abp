using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.RobotsTxts;

public class RobotsTxtManager : DomainService
{
    protected IRobotsTxtRepository RobotsTxtRepository { get; }

    protected IDistributedCache<RobotsTxtCacheItem> Cache { get; }

    public RobotsTxtManager(
        IRobotsTxtRepository robotsTxtRepository,
        IDistributedCache<RobotsTxtCacheItem> cache)
    {
        RobotsTxtRepository = robotsTxtRepository;
        Cache = cache;
    }

    /// <summary>
    /// Gets the robots.txt stored for the current tenant (or the host), without any fallback.
    /// </summary>
    public virtual async Task<RobotsTxt> FindAsync()
    {
        return await RobotsTxtRepository.FindAsync();
    }

    /// <summary>
    /// Creates or updates the robots.txt of the current tenant (or the host).
    /// </summary>
    public virtual async Task<RobotsTxt> SetAsync([CanBeNull] string content)
    {
        var robotsTxt = await RobotsTxtRepository.FindAsync();

        if (robotsTxt == null)
        {
            return await RobotsTxtRepository.InsertAsync(
                new RobotsTxt(GuidGenerator.Create(), content, CurrentTenant.Id)
            );
        }

        robotsTxt.SetContent(content);

        return await RobotsTxtRepository.UpdateAsync(robotsTxt);
    }

    /// <summary>
    /// Deletes the robots.txt of the current tenant (or the host),
    /// so the inherited (host) or default content is used.
    /// </summary>
    public virtual async Task ResetAsync()
    {
        var robotsTxt = await RobotsTxtRepository.FindAsync();
        if (robotsTxt != null)
        {
            await RobotsTxtRepository.DeleteAsync(robotsTxt);
        }
    }

    /// <summary>
    /// Gets the content of the current tenant. Falls back to the host's content.
    /// Returns null if neither the tenant nor the host has stored a content.
    /// </summary>
    public virtual async Task<string> GetEffectiveContentOrNullAsync()
    {
        var content = await GetCachedContentOrNullAsync();
        if (content != null || !CurrentTenant.IsAvailable)
        {
            return content;
        }

        using (CurrentTenant.Change(null))
        {
            return await GetCachedContentOrNullAsync();
        }
    }

    protected virtual async Task<string> GetCachedContentOrNullAsync()
    {
        var cacheItem = await Cache.GetOrAddAsync(
            RobotsTxtCacheItem.CacheKey, // Tenant-aware by default
            async () => new RobotsTxtCacheItem((await RobotsTxtRepository.FindAsync())?.Content)
        );

        return cacheItem?.Content;
    }
}
