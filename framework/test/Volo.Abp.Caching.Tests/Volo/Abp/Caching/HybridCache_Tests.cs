using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Caching.Hybrid;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Caching;

public class HybridCache_Tests : AbpIntegratedTest<AbpCachingTestModule>
{
    [Fact]
    public async Task Should_GetOrCreate_Set_And_Remove_Cache_Items()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

        var cacheKey = Guid.NewGuid().ToString();

        //GetOrCreateAsync
        var cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem("john nash")));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("john nash");

        //SetAsync
        await personCache.SetAsync(cacheKey, new PersonCacheItem("baris"));

        //GetOrCreateAsync
        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem("john nash")));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("baris");

        //RemoveAsync
        await personCache.RemoveAsync(cacheKey);

        //GetOrCreateAsync
        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem("lucas")));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("lucas");
    }

    [Fact]
    public async Task GetOrCreateAsync()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

        var cacheKey = Guid.NewGuid().ToString();
        const string personName = "john nash";

        //Will execute the factory method to create the cache item

        bool factoryExecuted = false;

        var cacheItem = await personCache.GetOrCreateAsync(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return Task.FromResult(new PersonCacheItem(personName));
            });

        factoryExecuted.ShouldBeTrue();
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);

        //This time, it will not execute the factory

        factoryExecuted = false;

        cacheItem = await personCache.GetOrCreateAsync(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return Task.FromResult(new PersonCacheItem(personName));
            });

        factoryExecuted.ShouldBeFalse();
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);
    }

    [Fact]
    public async Task SameClassName_But_DiffNamespace_Should_Not_Use_Same_Cache()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();
        var otherPersonCache = GetRequiredService<IHybridCache<Sail.Testing.Caching.PersonCacheItem>>();

        var cacheKey = Guid.NewGuid().ToString();
        const string personName = "john nash";

        var cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);
        var cacheItem1 = await otherPersonCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new Sail.Testing.Caching.PersonCacheItem(personName)));
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);

        await personCache.RemoveAsync(cacheKey);

        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName + "1")));
        cacheItem1 = await otherPersonCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new Sail.Testing.Caching.PersonCacheItem(personName + "1")));

        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName + "1");

        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);
    }

    [Fact]
    public async Task Should_Set_Get_And_Remove_Cache_Items_With_Integer_Type_CacheKey()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem, int>>();

        var cacheKey = 42;
        const string personName = "john nash";

        //GetOrCreateAsync
        var cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);

        //SetAsync
        await personCache.SetAsync(cacheKey, new PersonCacheItem("baris"));

        //GetOrCreateAsync
        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("baris");

        //RemoveAsync
        await personCache.RemoveAsync(cacheKey);

        //GetOrCreateAsync
        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem("lucas")));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("lucas");
    }

    [Fact]
    public async Task GetOrAddAsync_With_Integer_Type_CacheKey()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem, int>>();

        var cacheKey = 42;
        const string personName = "john nash";

        //Will execute the factory method to create the cache item

        bool factoryExecuted = false;

        var cacheItem = await personCache.GetOrCreateAsync(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return Task.FromResult(new PersonCacheItem(personName));
            });

        factoryExecuted.ShouldBeTrue();
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);

        //This time, it will not execute the factory

        factoryExecuted = false;

        cacheItem = await personCache.GetOrCreateAsync(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return Task.FromResult(new PersonCacheItem(personName));
            });

        factoryExecuted.ShouldBeFalse();
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);
    }

    [Fact]
    public async Task SameClassName_But_DiffNamespace_Should_Not_Use_Same_Cache_With_Integer_CacheKey()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem, int>>();
        var otherPersonCache = GetRequiredService<IHybridCache<Sail.Testing.Caching.PersonCacheItem, int>>();

        var cacheKey = 42;
        const string personName = "john nash";

        var cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);
        var cacheItem1 = await otherPersonCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new Sail.Testing.Caching.PersonCacheItem(personName)));
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);

        await personCache.RemoveAsync(cacheKey);

        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName + "1")));
        cacheItem1 = await otherPersonCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new Sail.Testing.Caching.PersonCacheItem(personName + "1")));

        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName + "1");

        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);
    }

    [Fact]
    public async Task Should_Set_Get_And_Remove_Cache_Items_With_Object_Type_CacheKey()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem, ComplexObjectAsCacheKey>>();

        var cacheKey = new ComplexObjectAsCacheKey { Name = "DummyData", Age = 42 };
        const string personName = "john nash";

        //GetOrCreateAsync
        var cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);

        //SetAsync
        await personCache.SetAsync(cacheKey, new PersonCacheItem("baris"));

        //GetOrCreateAsync
        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("baris");

        //RemoveAsync
        await personCache.RemoveAsync(cacheKey);

        //GetOrCreateAsync
        cacheItem = await personCache.GetOrCreateAsync(cacheKey, () => Task.FromResult(new PersonCacheItem("lucas")));
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe("lucas");
    }

    [Fact]
    public async Task Should_Set_Get_And_Remove_Cache_Items_For_Same_Object_Type_With_Different_CacheKeys()
    {
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem, ComplexObjectAsCacheKey>>();

        var cache1Key = new ComplexObjectAsCacheKey { Name = "John", Age = 42 };
        var cache2Key = new ComplexObjectAsCacheKey { Name = "Jenny", Age = 24 };
        const string personName = "john nash";

        //GetOrCreateAsync
        var cacheItem1 = await personCache.GetOrCreateAsync(cache1Key, () => Task.FromResult(new PersonCacheItem(personName)));
        var cacheItem2 = await personCache.GetOrCreateAsync(cache2Key, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);
        cacheItem2.ShouldNotBeNull();
        cacheItem2.Name.ShouldBe(personName);

        //SetAsync
        cacheItem1 = new PersonCacheItem("baris");
        cacheItem2 = new PersonCacheItem("jack");
        await personCache.SetAsync(cache1Key, cacheItem1);
        await personCache.SetAsync(cache2Key, cacheItem2);

        //GetOrCreateAsync
        cacheItem1 = await personCache.GetOrCreateAsync(cache1Key, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem2 = await personCache.GetOrCreateAsync(cache2Key, () => Task.FromResult(new PersonCacheItem(personName)));
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe("baris");
        cacheItem2.ShouldNotBeNull();
        cacheItem2.Name.ShouldBe("jack");

        //Remove
        await personCache.RemoveAsync(cache1Key);
        await personCache.RemoveAsync(cache2Key);

        //Get (not exists since removed)
        cacheItem1 = await personCache.GetOrCreateAsync(cache1Key, () => Task.FromResult(new PersonCacheItem("lucas")));
        cacheItem2 = await personCache.GetOrCreateAsync(cache2Key, () => Task.FromResult(new PersonCacheItem("peter")));
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe("lucas");
        cacheItem2.ShouldNotBeNull();
        cacheItem2.Name.ShouldBe("peter");
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_GetOrCreateAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

            var cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            await personCache.SetAsync(key, new PersonCacheItem("lucas"), considerUow: true);

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john2")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("lucas");

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john3")), considerUow: false);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john3");

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john4")), considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue.Name.ShouldBe("lucas");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_GetOrCreateAsync()
    {
        const string key = "testkey";
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: false);
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john2")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            await personCache.SetAsync(key, new PersonCacheItem("john3"), considerUow: true);

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john4")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john3");

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john5")), considerUow: false);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");
        }

        cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john6")), considerUow: false);
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_SetAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            var cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john2")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john3")), considerUow: false);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john3");

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john4")), considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue.Name.ShouldBe("john");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_SetAsync()
    {
        const string key = "testkey";
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: false);
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetAsync(key, new PersonCacheItem("john2"), considerUow: true);

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john3")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john2");

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john4")), considerUow: false);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");
        }

        cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john5")), considerUow: false);
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_RemoveAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            var cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john2")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            await personCache.RemoveAsync(key, considerUow: true);

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john3")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john3");

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john4")), considerUow: false);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john4");

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john5")), considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue.Name.ShouldBe("john3");
            });

            await uow.CompleteAsync();
        }
    }

    public async Task Cache_Should_Rollback_With_Uow_For_RemoveAsync()
    {
        const string key = "testkey";
        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: false);
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetAsync(key, new PersonCacheItem("john2"), considerUow: true);

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john2");

            await personCache.RemoveAsync(key, considerUow: true);

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john3")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john3")), considerUow: false);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");
        }

        cacheValue = await personCache.GetOrCreateAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: false);
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");
    }

    [Fact]
    public async Task Should_Remove_Multiple_Items_Async()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testkey3 = new[] { testkey, testkey2 };

        var personCache = GetRequiredService<IHybridCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetOrCreateAsync(testkey, () => Task.FromResult(new PersonCacheItem("john")));
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john");

        cacheValue = await personCache.GetOrCreateAsync(testkey2, () => Task.FromResult(new PersonCacheItem("jack")));
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("jack");

        await personCache.RemoveManyAsync(testkey3);

        cacheValue = await personCache.GetOrCreateAsync(testkey, () => Task.FromResult(new PersonCacheItem("john2")));
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("john2");

        cacheValue = await personCache.GetOrCreateAsync(testkey2, () => Task.FromResult(new PersonCacheItem("jack2")));
        cacheValue.ShouldNotBeNull();
        cacheValue.Name.ShouldBe("jack2");
    }

    [Fact]
    public async Task Should_Get_Same_Cache_Set_When_Resolve_With_Or_Without_Key()
    {
        var cache1 = GetRequiredService<IHybridCache<PersonCacheItem>>();
        var cache2 = GetRequiredService<IHybridCache<PersonCacheItem, string>>();

        cache1.InternalCache.ShouldBe(cache2);

        await cache1.SetAsync("john", new PersonCacheItem("john"));

        var item1 = await cache1.GetOrCreateAsync("john", () => Task.FromResult(new PersonCacheItem("john2")));
        item1.ShouldNotBeNull();
        item1.Name.ShouldBe("john");

        var item2 = await cache1.GetOrCreateAsync("john", () => Task.FromResult(new PersonCacheItem("john3")));
        item2.ShouldNotBeNull();
        item2.Name.ShouldBe("john");
    }
}
