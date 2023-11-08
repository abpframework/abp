using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributorCache : ITransientDependency
{
    public ILogger<IdentityDynamicClaimsPrincipalContributorCache> Logger { get; set; }

    protected IDistributedCache<List<AbpClaimCacheItem>> Cache { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IdentityUserManager UserManager { get; }
    protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; }
    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }
    protected IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> CacheOptions { get; }

    public IdentityDynamicClaimsPrincipalContributorCache(
        IDistributedCache<List<AbpClaimCacheItem>> cache,
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
        IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> cacheOptions)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
        UserManager = userManager;
        UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOptions;
        CacheOptions = cacheOptions;

        Logger = NullLogger<IdentityDynamicClaimsPrincipalContributorCache>.Instance;
    }

    public virtual async Task<List<AbpClaimCacheItem>> GetAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"Get dynamic claims cache for user: {userId}");

        return await Cache.GetOrAddAsync(AbpClaimCacheItem.CalculateCacheKey(userId, tenantId), async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                Logger.LogDebug($"Filling dynamic claims cache for user: {userId}");

                var user = await UserManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    Logger.LogWarning($"User not found: {userId}");
                    return new List<AbpClaimCacheItem>();
                }

                var principal = await UserClaimsPrincipalFactory.CreateAsync(user);
                return principal.Identities.FirstOrDefault()?.Claims
                    .Where(c => AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims.Contains(c.Type))
                    .Select(c => new AbpClaimCacheItem(c.Type, c.Value)).ToList();
            }
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
        });
    }

    public virtual async Task ClearAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"Clearing dynamic claims cache for user: {userId}");
        await Cache.RemoveAsync(AbpClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }
}
