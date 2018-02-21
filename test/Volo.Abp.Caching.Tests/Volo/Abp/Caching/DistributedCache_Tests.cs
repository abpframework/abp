using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Caching
{
    public class DistributedCache_Tests : AbpIntegratedTest<AbpCachingTestModule>
    {
        [Fact]
        public async Task Should_Set_Get_And_Remove_Cache_Items()
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheKey = Guid.NewGuid().ToString();
            const string personName = "john nash";

            //Get (not exists yet)
            var cacheItem = await personCache.GetAsync(cacheKey);
            cacheItem.ShouldBeNull();

            //Set
            cacheItem = new PersonCacheItem(personName);
            await personCache.SetAsync(cacheKey, cacheItem);

            //Get (it should be available now
            cacheItem = await personCache.GetAsync(cacheKey);
            cacheItem.ShouldNotBeNull();
            cacheItem.Name.ShouldBe(personName);

            //Remove 
            await personCache.RemoveAsync(cacheKey);

            //Get (not exists since removed)
            cacheItem = await personCache.GetAsync(cacheKey);
            cacheItem.ShouldBeNull();
        }
    }
}
