using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Nito.AsyncEx;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.Caching
{
    public class DistributedCache<TCacheItem> : IDistributedCache<TCacheItem>
        where TCacheItem : class
    {
        protected string CacheName { get; set; }

        protected bool IgnoreMultiTenancy { get; set; }

        protected IDistributedCache Cache { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        protected IObjectSerializer ObjectSerializer { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected AsyncLock AsyncLock { get; } = new AsyncLock();

        public DistributedCache(
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IObjectSerializer objectSerializer,
            ICurrentTenant currentTenant)
        {
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
            ObjectSerializer = objectSerializer;
            CurrentTenant = currentTenant;

            SetDefaultOptions();
        }

        public virtual TCacheItem Get(string key)
        {
            var cachedBytes = Cache.Get(NormalizeKey(key));
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        public virtual async Task<TCacheItem> GetAsync(string key, CancellationToken token = default)
        {
            var cachedBytes = await Cache.GetAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        public TCacheItem GetOrAdd(string key, Func<TCacheItem> factory)
        {
            var value = Get(key);
            if (value != null)
            {
                return value;
            }

            using (AsyncLock.Lock())
            {
                value = Get(key);
                if (value != null)
                {
                    return value;
                }

                value = factory();
                Set(key, value);

            }

            return value;
        }

        public async Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, CancellationToken token = default)
        {
            var value = await GetAsync(key, token);
            if (value != null)
            {
                return value;
            }

            using (await AsyncLock.LockAsync(token))
            {
                value = await GetAsync(key, token);
                if (value != null)
                {
                    return value;
                }

                value = await factory();
                await SetAsync(key, value, token: token);
            }

            return value;
        }

        public virtual void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null)
        {
            Cache.Set(
                NormalizeKey(key),
                ObjectSerializer.Serialize(value),
                options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) } //TODO: implement per cache item and global defaults!!!
            );
        }

        public virtual Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, CancellationToken token = default)
        {
            return Cache.SetAsync(
                NormalizeKey(key),
                ObjectSerializer.Serialize(value),
                options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) }, //TODO: implement per cache item and global defaults!!!
                CancellationTokenProvider.FallbackToProvider(token)
            );
        }

        public virtual void Refresh(string key)
        {
            Cache.Refresh(NormalizeKey(key));
        }

        public virtual Task RefreshAsync(string key, CancellationToken token = default)
        {
            return Cache.RefreshAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(NormalizeKey(key));
        }

        public virtual Task RemoveAsync(string key, CancellationToken token = default)
        {
            return Cache.RemoveAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
        }

        protected virtual string NormalizeKey(string key)
        {
            var normalizedKey = "c:" + CacheName + ",k:" + key;

            if (!IgnoreMultiTenancy && CurrentTenant.Id.HasValue)
            {
                normalizedKey = "t:" + CurrentTenant.Id.Value + "," + normalizedKey;
            }

            return normalizedKey;
        }

        protected virtual void SetDefaultOptions()
        {
            //CacheName
            var cacheNameAttribute = typeof(TCacheItem)
                .GetCustomAttributes(true)
                .OfType<CacheNameAttribute>()
                .FirstOrDefault();

            CacheName = cacheNameAttribute != null ? cacheNameAttribute.Name : typeof(TCacheItem).Name;

            //IgnoreMultiTenancy
            IgnoreMultiTenancy = typeof(TCacheItem).IsDefined(typeof(IgnoreMultiTenancyAttribute), true);
        }
    }
}