using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.Caching
{
    public class CacheNameAttribute_Tests
    {
        [Fact]
        public void CacheNameAttribute_Test()
        {
            CacheNameAttribute.GetCacheName(typeof(MyCacheItem)).ShouldBe("My_Cache_Item");
        }

        [Fact]
        public void ICacheNameProvider_Test()
        {
            CacheNameAttribute.GetCacheName(typeof(MyCacheItem2<MyCacheItem>)).ShouldBe(nameof(MyCacheItem2<MyCacheItem>) + typeof(MyCacheItem).Name);
        }

        [Fact]
        public void ICacheNameProvider_Exception_Test()
        {
            var exception = Assert.Throws<AbpException>(() => CacheNameAttribute.GetCacheName(typeof(ICacheNameProvider)));
            exception.Message.ShouldContain($"Cannot create an instance of type {typeof(ICacheNameProvider).FullName}.");
        }

        [Serializable]
        [CacheName("My_Cache_Item")]
        public class MyCacheItem
        {
            public string Value { get; set; }

            public MyCacheItem()
            {

            }

            public MyCacheItem(string value)
            {
                Value = value;
            }
        }

        [Serializable]
        public class MyCacheItem2<T> : ICacheNameProvider
        {
            public T Value { get; set; }

            public MyCacheItem2()
            {

            }

            public MyCacheItem2(T value)
            {
                Value = value;
            }

            public string GetCacheName()
            {
                return nameof(MyCacheItem2<T>) + typeof(T).Name;
            }
        }
    }
}
