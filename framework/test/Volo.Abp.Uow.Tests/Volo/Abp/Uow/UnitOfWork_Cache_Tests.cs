using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Uow
{
    public class CacheItem
    {
        public string Value { get; set; }

        public CacheItem(string value)
        {
            Value = value;
        }
    }

    public class MyUnitOfWorkCache : UnitOfWorkCache<CacheItem>, ITransientDependency
    {
        public MyUnitOfWorkCache(IUnitOfWorkManager unitOfWorkManager)
            : base(unitOfWorkManager)
        {
        }

        public override string Name => "MyUnitOfWorkCache";
    }

    public class MyUnitOfWorkCacheWithFallback : UnitOfWorkCacheWithFallback<CacheItem>, ITransientDependency
    {
        public MyUnitOfWorkCacheWithFallback(IUnitOfWorkManager unitOfWorkManager)
            : base(unitOfWorkManager)
        {
        }

        public override string Name => "MyUnitOfWorkCacheWithFallback";

        public Dictionary<string, CacheItem> FalbackCache = new Dictionary<string, CacheItem>();

        public override Task<CacheItem> GetFallbackCacheItem(string key)
        {
            return Task.FromResult(FalbackCache.GetOrDefault(key));
        }

        public override Task<CacheItem> SetFallbackCacheItem(string key, CacheItem item)
        {
            FalbackCache.Add(key, item);
            return Task.FromResult(item);
        }
    }


    public class UnitOfWork_Cache_Tests : AbpIntegratedTest<AbpUnitOfWorkTestModule>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly MyUnitOfWorkCache _myUnitOfWorkCache;

        public UnitOfWork_Cache_Tests()
        {
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            _myUnitOfWorkCache = ServiceProvider.GetRequiredService<MyUnitOfWorkCache>();
        }

        [Fact]
        public async Task GetOrAddAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var cacheValue = await _myUnitOfWorkCache.GetOrAddAsync("testkey", () => Task.FromResult(new CacheItem("testvalue")));
                cacheValue.Value.ShouldBe("testvalue");
            }
        }

        [Fact]
        public async Task GetOrNullAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.ShouldBeNull();

                await _myUnitOfWorkCache.SetAsync("testkey", new CacheItem("testvalue"));

                cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue");
            }
        }

        [Fact]
        public async Task SetAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _myUnitOfWorkCache.SetAsync("testkey", new CacheItem("testvalue"));

                var cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue");
            }
        }

        [Fact]
        public async Task Set_Multiple()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _myUnitOfWorkCache.SetAsync("testkey", new CacheItem("testvalue"));

                var cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue");

                await _myUnitOfWorkCache.SetAsync("testkey", new CacheItem("testvalue2"));
                cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue2");
            }
        }

        [Fact]
        public async Task RemoveAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _myUnitOfWorkCache.SetAsync("testkey", new CacheItem("testvalue"));

                var cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue");

                await _myUnitOfWorkCache.RemoveAsync("testkey");

                cacheValue = await _myUnitOfWorkCache.GetOrNullAsync("testkey");
                cacheValue.ShouldBeNull();
            }
        }
    }

    public class UnitOfWorkWithFallback_Cache_Tests : AbpIntegratedTest<AbpUnitOfWorkTestModule>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly MyUnitOfWorkCacheWithFallback _myUnitOfWorkCacheWithFallback;

        public UnitOfWorkWithFallback_Cache_Tests()
        {
            _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            _myUnitOfWorkCacheWithFallback = ServiceProvider.GetRequiredService<MyUnitOfWorkCacheWithFallback>();
        }

        [Fact]
        public async Task GetOrNullAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var cacheValue = await _myUnitOfWorkCacheWithFallback.GetOrNullAsync("testkey");
                cacheValue.ShouldBeNull();

                _myUnitOfWorkCacheWithFallback.FalbackCache.Add("testkey", new CacheItem("testvalue"));

                cacheValue = await _myUnitOfWorkCacheWithFallback.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue");
            }
        }

        [Fact]
        public async Task GetOrAddAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var cacheValue = await _myUnitOfWorkCacheWithFallback.GetOrAddAsync("testkey", () => Task.FromResult(new CacheItem("testvalue")));
                cacheValue.Value.ShouldBe("testvalue");

                _myUnitOfWorkCacheWithFallback.FalbackCache.ShouldContain(x => x.Key == "testkey" && x.Value.Value == "testvalue");
            }
        }

        [Fact]
        public async Task RemoveAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _myUnitOfWorkCacheWithFallback.SetAsync("testkey", new CacheItem("testvalue"));
                _myUnitOfWorkCacheWithFallback.FalbackCache.Add("testkey", new CacheItem("testvalue"));

                var cacheValue = await _myUnitOfWorkCacheWithFallback.GetOrNullAsync("testkey");
                cacheValue.Value.ShouldBe("testvalue");

                await _myUnitOfWorkCacheWithFallback.RemoveAsync("testkey");

                cacheValue = await _myUnitOfWorkCacheWithFallback.GetOrNullAsync("testkey");
                cacheValue.ShouldBeNull();
            }
        }
    }
}
