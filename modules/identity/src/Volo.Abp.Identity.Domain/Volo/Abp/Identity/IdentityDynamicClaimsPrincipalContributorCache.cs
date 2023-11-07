using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributorCache : ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public IdentityDynamicClaimsPrincipalContributorCache(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public virtual async Task<List<AbpClaimCacheItem>> GetAsync(Guid userId, Guid? tenantId = null)
    {
        var logger = ServiceProvider.GetRequiredService<ILogger<IdentityDynamicClaimsPrincipalContributorCache>>();
        logger.LogDebug($"Get dynamic claims cache for user: {userId}");

        var cache = ServiceProvider.GetRequiredService<IDistributedCache<List<AbpClaimCacheItem>>>();

        return await cache.GetOrAddAsync($"{nameof(IdentityDynamicClaimsPrincipalContributorCache)}_{tenantId}_{userId}", async () =>
        {
            using (ServiceProvider.GetRequiredService<ICurrentTenant>().Change(tenantId))
            {
                logger.LogDebug($"Filling dynamic claims cache for user: {userId}");
                var userManager = ServiceProvider.GetRequiredService<IdentityUserManager>();
                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    logger.LogWarning($"User not found: {userId}");
                    return new List<AbpClaimCacheItem>();
                }
                var factory = ServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
                var principal = await factory.CreateAsync(user);
                var options = ServiceProvider.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value;
                return principal.Identities.FirstOrDefault()?.Claims.Where(c => options.DynamicClaims.Contains(c.Type)).Select(c => new AbpClaimCacheItem(c.Type, c.Value)).ToList();
            }
        });
    }

    public virtual async Task ClearAsync(Guid userId, Guid? tenantId = null)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<List<AbpClaimCacheItem>>>();
        var logger = ServiceProvider.GetRequiredService<ILogger<IdentityDynamicClaimsPrincipalContributorCache>>();
        logger.LogDebug($"Clearing dynamic claims cache for user: {userId}");
        await cache.RemoveAsync($"{nameof(IdentityDynamicClaimsPrincipalContributorCache)}_{tenantId}_{userId}");
    }
}
