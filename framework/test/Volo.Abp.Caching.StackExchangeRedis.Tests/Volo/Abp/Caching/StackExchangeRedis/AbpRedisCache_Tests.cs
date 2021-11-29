using Microsoft.Extensions.Caching.Distributed;
using Shouldly;
using Xunit;

namespace Volo.Abp.Caching.StackExchangeRedis
{
    public class AbpRedisCache_Tests : AbpCachingStackExchangeRedisTestBase
    {
        private readonly IDistributedCache _distributedCache;
        
        public AbpRedisCache_Tests()
        {
            _distributedCache = GetRequiredService<IDistributedCache>();
        }

        [Fact]
        public void Should_Replace_RedisCache()
        {
            (_distributedCache is AbpRedisCache).ShouldBeTrue();
        }
    }
}