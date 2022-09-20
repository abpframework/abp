using Microsoft.Extensions.Caching.Distributed;
using Shouldly;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Caching;

public class DistributedCache_ConfigureOptions_Test : AbpIntegratedTest<AbpCachingTestModule>
{
    [Fact]
    public void Configure_AbpCacheOptions()
    {
        var personCache = GetRequiredService<IDistributedCache<Sail.Testing.Caching.PersonCacheItem>>();
        GetDefaultCachingOptions(personCache).SlidingExpiration.ShouldBeNull();
        GetDefaultCachingOptions(personCache).AbsoluteExpiration.ShouldBe(new DateTime(2099, 1, 1, 12, 0, 0));
    }

    [Fact]
    public async Task Default_AbpCacheOptions_Should_Be_20_Mins()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheKey = Guid.NewGuid().ToString();

        //Get (not exists yet)
        var cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldBeNull();

        GetDefaultCachingOptions(personCache).SlidingExpiration.ShouldBe(TimeSpan.FromMinutes(20));

    }
    private static DistributedCacheEntryOptions GetDefaultCachingOptions(object instance)
    {
        var defaultOptionsField = instance.GetType().GetTypeInfo().GetField("DefaultCacheOptions", BindingFlags.Instance | BindingFlags.NonPublic);
        return (DistributedCacheEntryOptions)defaultOptionsField.GetValue(instance);
    }
}
