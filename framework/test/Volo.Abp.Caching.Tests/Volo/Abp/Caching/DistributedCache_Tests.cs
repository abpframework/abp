using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Caching;

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

    [Fact]
    public void GetOrAdd()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheKey = Guid.NewGuid().ToString();
        const string personName = "john nash";

        //Will execute the factory method to create the cache item

        bool factoryExecuted = false;

        var cacheItem = personCache.GetOrAdd(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return new PersonCacheItem(personName);
            });

        factoryExecuted.ShouldBeTrue();
        cacheItem.Name.ShouldBe(personName);

        //This time, it will not execute the factory

        factoryExecuted = false;

        cacheItem = personCache.GetOrAdd(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return new PersonCacheItem(personName);
            });

        factoryExecuted.ShouldBeFalse();
        cacheItem.Name.ShouldBe(personName);
    }

    [Fact]
    public async Task SameClassName_But_DiffNamespace_Should_Not_Use_Same_Cache()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();
        var otherPersonCache = GetRequiredService<IDistributedCache<Sail.Testing.Caching.PersonCacheItem>>();

        var cacheKey = Guid.NewGuid().ToString();
        const string personName = "john nash";

        //Get (not exists yet)
        var cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldBeNull();

        var cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldBeNull();

        //Set
        cacheItem = new PersonCacheItem(personName);
        await personCache.SetAsync(cacheKey, cacheItem);

        //Get (it should be available now, but otherPersonCache not exists now.
        cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);

        cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldBeNull();

        //set other person cache
        cacheItem1 = new Sail.Testing.Caching.PersonCacheItem(personName);
        await otherPersonCache.SetAsync(cacheKey, cacheItem1);

        cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);

        //Remove
        await personCache.RemoveAsync(cacheKey);


        //Get (not exists since removed)
        cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldBeNull();

        cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldNotBeNull();

    }

    [Fact]
    public async Task Should_Set_Get_And_Remove_Cache_Items_With_Integer_Type_CacheKey()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem, int>>();

        var cacheKey = 42;
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

    [Fact]
    public async Task GetOrAddAsync_With_Integer_Type_CacheKey()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem, int>>();

        var cacheKey = 42;
        const string personName = "john nash";

        //Will execute the factory method to create the cache item

        bool factoryExecuted = false;

        var cacheItem = personCache.GetOrAdd(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return new PersonCacheItem(personName);
            });

        factoryExecuted.ShouldBeTrue();
        cacheItem.Name.ShouldBe(personName);

        //This time, it will not execute the factory

        factoryExecuted = false;

        cacheItem = await personCache.GetOrAddAsync(cacheKey,
            () =>
            {
                factoryExecuted = true;
                return Task.FromResult(new PersonCacheItem(personName));
            });

        factoryExecuted.ShouldBeFalse();
        cacheItem.Name.ShouldBe(personName);
    }

    [Fact]
    public async Task SameClassName_But_DiffNamespace_Should_Not_Use_Same_Cache_With_Integer_CacheKey()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem, int>>();
        var otherPersonCache = GetRequiredService<IDistributedCache<Sail.Testing.Caching.PersonCacheItem, int>>();


        var cacheKey = 42;
        const string personName = "john nash";

        //Get (not exists yet)
        var cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldBeNull();

        var cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldBeNull();

        //Set
        cacheItem = new PersonCacheItem(personName);
        await personCache.SetAsync(cacheKey, cacheItem);

        //Get (it should be available now, but otherPersonCache not exists now.
        cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldNotBeNull();
        cacheItem.Name.ShouldBe(personName);

        cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldBeNull();

        //set other person cache
        cacheItem1 = new Sail.Testing.Caching.PersonCacheItem(personName);
        await otherPersonCache.SetAsync(cacheKey, cacheItem1);

        cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);

        //Remove
        await personCache.RemoveAsync(cacheKey);


        //Get (not exists since removed)
        cacheItem = await personCache.GetAsync(cacheKey);
        cacheItem.ShouldBeNull();

        cacheItem1 = await otherPersonCache.GetAsync(cacheKey);
        cacheItem1.ShouldNotBeNull();

    }

    [Fact]
    public async Task Should_Set_Get_And_Remove_Cache_Items_With_Object_Type_CacheKey()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem, ComplexObjectAsCacheKey>>();

        var cacheKey = new ComplexObjectAsCacheKey { Name = "DummyData", Age = 42 };
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

    [Fact]
    public async Task Should_Set_Get_And_Remove_Cache_Items_For_Same_Object_Type_With_Different_CacheKeys()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem, ComplexObjectAsCacheKey>>();

        var cache1Key = new ComplexObjectAsCacheKey { Name = "John", Age = 42 };
        var cache2Key = new ComplexObjectAsCacheKey { Name = "Jenny", Age = 24 };
        const string personName = "john nash";

        //Get (not exists yet)
        var cacheItem1 = await personCache.GetAsync(cache1Key);
        var cacheItem2 = await personCache.GetAsync(cache2Key);
        cacheItem1.ShouldBeNull();
        cacheItem2.ShouldBeNull();

        //Set
        cacheItem1 = new PersonCacheItem(personName);
        cacheItem2 = new PersonCacheItem(personName);
        await personCache.SetAsync(cache1Key, cacheItem1);
        await personCache.SetAsync(cache2Key, cacheItem2);

        //Get (it should be available now
        cacheItem1 = await personCache.GetAsync(cache1Key);
        cacheItem1.ShouldNotBeNull();
        cacheItem1.Name.ShouldBe(personName);

        cacheItem2 = await personCache.GetAsync(cache2Key);
        cacheItem2.ShouldNotBeNull();
        cacheItem2.Name.ShouldBe(personName);

        //Remove
        await personCache.RemoveAsync(cache1Key);
        await personCache.RemoveAsync(cache2Key);

        //Get (not exists since removed)
        cacheItem1 = await personCache.GetAsync(cache1Key);
        cacheItem1.ShouldBeNull();
        cacheItem2 = await personCache.GetAsync(cache2Key);
        cacheItem2.ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_GetAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldBeNull();

            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: false);
            cacheValue.ShouldBeNull();

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetAsync(key, considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue.Name.ShouldBe("john");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_GetAsync()
    {
        const string key = "testkey";
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldBeNull();

            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: false);
            cacheValue.ShouldBeNull();
        }

        cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_GetOrAddAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheValue = await personCache.GetOrAddAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: false);
            cacheValue.ShouldBeNull();

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetAsync(key, considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue.Name.ShouldBe("john");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_GetOrAddAsync()
    {
        const string key = "testkey";
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            cacheValue = await personCache.GetOrAddAsync(key, () => Task.FromResult(new PersonCacheItem("john")), considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: false);
            cacheValue.ShouldBeNull();
        }

        cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_SetAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            var cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: false);
            cacheValue.ShouldBeNull();

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetAsync(key, considerUow: false);
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
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            cacheValue = await personCache.GetAsync(key, considerUow: false);
            cacheValue.ShouldBeNull();
        }

        cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_RemoveAsync()
    {
        const string key = "testkey";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            var cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            await personCache.RemoveAsync(key, considerUow: true);

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldBeNull();

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetAsync(key, considerUow: false);
                cacheValue.ShouldBeNull();
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_RemoveAsync()
    {
        const string key = "testkey";
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetAsync(key, new PersonCacheItem("john"), considerUow: true);

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldNotBeNull();
            cacheValue.Name.ShouldBe("john");

            await personCache.RemoveAsync(key, considerUow: true);

            cacheValue = await personCache.GetAsync(key, considerUow: true);
            cacheValue.ShouldBeNull();
        }

        cacheValue = await personCache.GetAsync(key, considerUow: false);
        cacheValue.ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_GetManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testKeys = new[] { testkey, testkey2 };

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheValue = await personCache.GetManyAsync(testKeys, considerUow: true);
            cacheValue.Where(x => x.Value != null).ShouldBeEmpty();

            await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
               {
                   new KeyValuePair<string, PersonCacheItem>(testkey, new PersonCacheItem("john")),
                   new KeyValuePair<string, PersonCacheItem>(testkey2, new PersonCacheItem("jack"))
               }, considerUow: true);

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: true);
            cacheValue.Where(x => x.Value != null).ShouldNotBeEmpty();
            cacheValue.ShouldContain(x => x.Value.Name == "john" || x.Value.Name == "jack");

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
            cacheValue.Where(x => x.Value != null).ShouldBeEmpty();

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
                cacheValue.ShouldNotBeEmpty();
                cacheValue.ShouldContain(x => x.Value.Name == "john" || x.Value.Name == "jack");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_GetManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testKeys = new[] { testkey, testkey2 };

        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
        cacheValue.Where(x => x.Value != null).ShouldBeEmpty();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: true);
            cacheValue.Where(x => x.Value != null).ShouldBeEmpty();

            await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new KeyValuePair<string, PersonCacheItem>(testkey, new PersonCacheItem("john")),
                    new KeyValuePair<string, PersonCacheItem>(testkey2, new PersonCacheItem("jack"))
                }, considerUow: true);

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: true);
            cacheValue.Where(x => x.Value != null).ShouldNotBeEmpty();
            cacheValue.ShouldContain(x => x.Value.Name == "john" || x.Value.Name == "jack");

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
            cacheValue.Where(x => x.Value != null).ShouldBeEmpty();
        }

        cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
        cacheValue.Where(x => x.Value != null).ShouldBeEmpty();
    }


    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_SetManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testKeys = new[] { testkey, testkey2 };

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new KeyValuePair<string, PersonCacheItem>(testkey, new PersonCacheItem("john")),
                    new KeyValuePair<string, PersonCacheItem>(testkey, new PersonCacheItem("jack"))
                }, considerUow: true);

            var cacheValue = await personCache.GetManyAsync(testKeys, considerUow: true);
            cacheValue.Where(x => x.Value != null).ShouldNotBeEmpty();
            cacheValue.ShouldContain(x => x.Value.Name == "john" || x.Value.Name == "jack");

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
            cacheValue.Where(x => x.Value != null).ShouldBeEmpty();

            uow.OnCompleted(async () =>
            {
                var cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
                cacheValue.Where(x => x.Value != null).ShouldNotBeEmpty();
                cacheValue.ShouldContain(x => x.Value.Name == "john" || x.Value.Name == "jack");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_SetManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testKeys = new[] { testkey, testkey2 };

        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
        cacheValue.Where(x => x.Value != null).ShouldBeEmpty();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new KeyValuePair<string, PersonCacheItem>(testkey, new PersonCacheItem("john")),
                    new KeyValuePair<string, PersonCacheItem>(testkey, new PersonCacheItem("jack"))
                }, considerUow: true);

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: true);
            cacheValue.Where(x => x.Value != null).ShouldNotBeEmpty();
            cacheValue.ShouldContain(x => x.Value.Name == "john" || x.Value.Name == "jack");

            cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
            cacheValue.Where(x => x.Value != null).ShouldBeEmpty();
        }

        cacheValue = await personCache.GetManyAsync(testKeys, considerUow: false);
        cacheValue.Where(x => x.Value != null).ShouldBeEmpty();
    }


    [Fact]
    public async Task Should_Set_And_Get_Multiple_Items_Async()
    {
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        await personCache.SetManyAsync(new[]
        {
                new KeyValuePair<string, PersonCacheItem>("john", new PersonCacheItem("John Nash")),
                new KeyValuePair<string, PersonCacheItem>("thomas", new PersonCacheItem("Thomas Moore"))
            });

        var cacheItems = await personCache.GetManyAsync(new[]
        {
                "john",
                "thomas",
                "baris" //doesn't exist
            });

        cacheItems.Length.ShouldBe(3);
        cacheItems[0].Key.ShouldBe("john");
        cacheItems[0].Value.Name.ShouldBe("John Nash");
        cacheItems[1].Key.ShouldBe("thomas");
        cacheItems[1].Value.Name.ShouldBe("Thomas Moore");
        cacheItems[2].Key.ShouldBe("baris");
        cacheItems[2].Value.ShouldBeNull();

        (await personCache.GetAsync("john")).Name.ShouldBe("John Nash");
        (await personCache.GetAsync("baris")).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Get_And_Add_Multiple_Items_Async()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";

        var testkey3 = new[] { testkey, testkey2 };
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        await personCache.SetAsync(testkey, new PersonCacheItem("john"));

        var cacheValue = await personCache.GetManyAsync(testkey3);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.Name.ShouldBe("john");
        cacheValue[1].Value.ShouldBeNull();

        cacheValue = await personCache.GetOrAddManyAsync(testkey3, (missingKeys) =>
        {
            var missingKeyArray = missingKeys.ToArray();
            missingKeyArray.Length.ShouldBe(1);
            missingKeyArray[0].ShouldBe(testkey2);

            return Task.FromResult(new List<KeyValuePair<string, PersonCacheItem>>
            {
                    new(testkey2, new PersonCacheItem("jack"))
            });
        });

        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.Name.ShouldBe("john");
        cacheValue[1].Value.Name.ShouldBe("jack");

        cacheValue = await personCache.GetManyAsync(testkey3);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.Name.ShouldBe("john");
        cacheValue[1].Value.Name.ShouldBe("jack");
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_GetOrAddManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";

        var testkey3 = new[] { testkey, testkey2 };

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

            var cacheValue = await personCache.GetOrAddManyAsync(testkey3, (missingKeys) => Task.FromResult(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new(testkey, new PersonCacheItem("john")),
                    new(testkey2, new PersonCacheItem("jack")),
                }), considerUow: true);

            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: true);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.ShouldBeNull();
            cacheValue[1].Value.ShouldBeNull();

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue[0].Value.Name.ShouldBe("john");
                cacheValue[1].Value.Name.ShouldBe("jack");
            });

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_GetOrAddManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";

        var testkey3 = new[] { testkey, testkey2 };

        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.ShouldBeNull();
        cacheValue[1].Value.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            cacheValue = await personCache.GetOrAddManyAsync(testkey3, (missingKeys) => Task.FromResult(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new(testkey, new PersonCacheItem("john")),
                    new(testkey2, new PersonCacheItem("jack")),
                }), considerUow: true);

            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: true);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.ShouldBeNull();
            cacheValue[1].Value.ShouldBeNull();
        }

        cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.ShouldBeNull();
        cacheValue[1].Value.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Remove_Multiple_Items_Async()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testkey3 = new[] { testkey, testkey2 };

        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
            {
                new(testkey, new PersonCacheItem("john")),
                new(testkey2, new PersonCacheItem("jack"))
            });

        await personCache.RemoveManyAsync(testkey3);

        var cacheValue = await personCache.GetManyAsync(testkey3);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.ShouldBeNull();
        cacheValue[1].Value.ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Only_Available_In_Uow_For_RemoveManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testkey3 = new[] { testkey, testkey2 };
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.ShouldBeNull();
        cacheValue[1].Value.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new(testkey, new PersonCacheItem("john")),
                    new(testkey2, new PersonCacheItem("jack"))
                });

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: true);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            await personCache.RemoveManyAsync(testkey3, considerUow: true);

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            uow.OnCompleted(async () =>
            {
                cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
                cacheValue.ShouldNotBeNull();
                cacheValue[0].Value.ShouldBeNull();
                cacheValue[1].Value.ShouldBeNull();
            });

            await uow.CompleteAsync();
        }

    }

    [Fact]
    public async Task Cache_Should_Rollback_With_Uow_For_RemoveManyAsync()
    {
        var testkey = "testkey";
        var testkey2 = "testkey2";
        var testkey3 = new[] { testkey, testkey2 };
        var personCache = GetRequiredService<IDistributedCache<PersonCacheItem>>();

        var cacheValue = await personCache.GetManyAsync(testkey3, considerUow: false);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.ShouldBeNull();
        cacheValue[1].Value.ShouldBeNull();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await personCache.SetManyAsync(new List<KeyValuePair<string, PersonCacheItem>>
                {
                    new(testkey, new PersonCacheItem("john")),
                    new(testkey2, new PersonCacheItem("jack"))
                }, considerUow: true);

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: true);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.Name.ShouldBe("john");
            cacheValue[1].Value.Name.ShouldBe("jack");

            await personCache.RemoveManyAsync(testkey3, considerUow: true);

            cacheValue = await personCache.GetManyAsync(testkey3, considerUow: true);
            cacheValue.Length.ShouldBe(2);
            cacheValue[0].Value.ShouldBeNull();
            cacheValue[1].Value.ShouldBeNull();
        }

        cacheValue = await personCache.GetManyAsync(testkey3, considerUow: true);
        cacheValue.Length.ShouldBe(2);
        cacheValue[0].Value.ShouldBeNull();
        cacheValue[1].Value.ShouldBeNull();
    }
}
