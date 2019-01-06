using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
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

        protected DistributedCacheEntryOptions DefaultCacheOptions;

        private readonly CacheOptions _cacheOption;
        public DistributedCache(
            IOptions<CacheOptions> cacheOption,
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IObjectSerializer objectSerializer,
            ICurrentTenant currentTenant)
        {
            _cacheOption = cacheOption.Value;
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

        public TCacheItem GetOrAdd(
            string key,
            Func<TCacheItem> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null)
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
                Set(key, value, optionsFactory?.Invoke());
            }

            return value;
        }

        public async Task<TCacheItem> GetOrAddAsync(
            string key,
            Func<Task<TCacheItem>> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            CancellationToken token = default)
        {
            token = CancellationTokenProvider.FallbackToProvider(token);
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
                await SetAsync(key, value, optionsFactory?.Invoke(), token);
            }

            return value;
        }

        public virtual void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null)
        {
            Cache.Set(
                NormalizeKey(key),
                ObjectSerializer.Serialize(value),
                options ?? DefaultCacheOptions
            );
        }

        public virtual Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, CancellationToken token = default)
        {
            return Cache.SetAsync(
                NormalizeKey(key),
                ObjectSerializer.Serialize(value),
                options ?? DefaultCacheOptions,
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
        protected virtual DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
        {
            foreach (var configure in _cacheOption.CacheConfigurators)
            {
                var options = configure.Invoke(CacheName);
                if (options != null)
                {
                    return options;
                }
            }
            return _cacheOption.GlobalCacheEntryOptions;
        }

        protected virtual void SetDefaultOptions()
        {
            //CacheName
            var cacheNameAttribute = typeof(TCacheItem)
                .GetCustomAttributes(true)
                .OfType<CacheNameAttribute>()
                .FirstOrDefault();

            CacheName = cacheNameAttribute != null ? cacheNameAttribute.Name : typeof(TCacheItem).FullName;

            //IgnoreMultiTenancy
            IgnoreMultiTenancy = typeof(TCacheItem).IsDefined(typeof(IgnoreMultiTenancyAttribute), true);

            //Configure default cache entry options
            DefaultCacheOptions = GetDefaultCacheEntryOptions();
        }
    }
}