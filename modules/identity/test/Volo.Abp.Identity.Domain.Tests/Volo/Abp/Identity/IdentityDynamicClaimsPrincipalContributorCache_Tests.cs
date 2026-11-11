using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributorCache_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityDynamicClaimsPrincipalContributorCache _cache;
    private readonly IDistributedCache<AbpDynamicClaimCacheItem> _distributedCache;
    private readonly IdentityTestData _testData;

    public IdentityDynamicClaimsPrincipalContributorCache_Tests()
    {
        _cache = GetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();
        _distributedCache = GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task RefreshAsync_Should_Overwrite_Cache_Without_Clearing_First()
    {
        var cacheKey = AbpDynamicClaimCacheItem.CalculateCacheKey(_testData.UserJohnId, null);

        await _distributedCache.SetAsync(cacheKey, new AbpDynamicClaimCacheItem());

        await _cache.RefreshAsync(_testData.UserJohnId);

        var item = await _distributedCache.GetAsync(cacheKey);
        item.ShouldNotBeNull();
        item.Claims.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task RefreshAsync_Should_Keep_Cache_Populated_Across_Parallel_Calls()
    {
        var cacheKey = AbpDynamicClaimCacheItem.CalculateCacheKey(_testData.UserJohnId, null);

        await Task.WhenAll(
            _cache.RefreshAsync(_testData.UserJohnId),
            _cache.RefreshAsync(_testData.UserJohnId),
            _cache.RefreshAsync(_testData.UserJohnId));

        var item = await _distributedCache.GetAsync(cacheKey);
        item.ShouldNotBeNull();
        item.Claims.ShouldNotBeEmpty();
    }
}
