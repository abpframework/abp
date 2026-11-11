using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.Security.Claims;

public class RemoteDynamicClaimsPrincipalContributorCacheBase_Tests
{
    [Fact]
    public async Task GetAsync_Should_Retry_Refresh_When_Cache_Is_Empty_After_First_Refresh()
    {
        var userId = Guid.NewGuid();
        var cacheItem = new AbpDynamicClaimCacheItem();
        var cache = new TestRemoteDynamicClaimsPrincipalContributorCache(
            Microsoft.Extensions.Options.Options.Create(new AbpClaimsPrincipalFactoryOptions()),
            missCountAfterRefresh: 1,
            cacheItem);

        var result = await cache.GetAsync(userId);

        result.ShouldBe(cacheItem);
        cache.RefreshCallCount.ShouldBe(2);
    }

    [Fact]
    public async Task GetAsync_Should_Throw_When_Cache_Is_Still_Empty_After_Retry()
    {
        var userId = Guid.NewGuid();
        var cache = new TestRemoteDynamicClaimsPrincipalContributorCache(
            Microsoft.Extensions.Options.Options.Create(new AbpClaimsPrincipalFactoryOptions()),
            missCountAfterRefresh: 2,
            cacheItem: new AbpDynamicClaimCacheItem());

        var exception = await Assert.ThrowsAsync<AbpException>(() => cache.GetAsync(userId));

        exception.Message.ShouldContain(userId.ToString());
        exception.Message.ShouldContain("from cache after refreshing");
        cache.RefreshCallCount.ShouldBe(2);
    }

    private class TestRemoteDynamicClaimsPrincipalContributorCache
        : RemoteDynamicClaimsPrincipalContributorCacheBase<TestRemoteDynamicClaimsPrincipalContributorCache>
    {
        private readonly int _missCountAfterRefresh;
        private readonly AbpDynamicClaimCacheItem _cacheItem;
        private int _getAfterRefreshCount;

        public int RefreshCallCount { get; private set; }

        public TestRemoteDynamicClaimsPrincipalContributorCache(
            IOptions<AbpClaimsPrincipalFactoryOptions> options,
            int missCountAfterRefresh,
            AbpDynamicClaimCacheItem cacheItem)
            : base(options)
        {
            _missCountAfterRefresh = missCountAfterRefresh;
            _cacheItem = cacheItem;
        }

        protected override Task<AbpDynamicClaimCacheItem?> GetCacheAsync(Guid userId, Guid? tenantId = null)
        {
            if (RefreshCallCount == 0)
            {
                return Task.FromResult<AbpDynamicClaimCacheItem?>(null);
            }

            _getAfterRefreshCount++;
            if (_getAfterRefreshCount <= _missCountAfterRefresh)
            {
                return Task.FromResult<AbpDynamicClaimCacheItem?>(null);
            }

            return Task.FromResult<AbpDynamicClaimCacheItem?>(_cacheItem);
        }

        protected override Task RefreshAsync(Guid userId, Guid? tenantId = null)
        {
            RefreshCallCount++;
            return Task.CompletedTask;
        }
    }
}
