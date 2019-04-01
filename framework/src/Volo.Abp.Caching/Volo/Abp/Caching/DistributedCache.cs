using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        public ILogger<DistributedCache<TCacheItem>> Logger { get; set; }

        protected string CacheName { get; set; }

        protected bool IgnoreMultiTenancy { get; set; }

        protected IDistributedCache Cache { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        protected IObjectSerializer ObjectSerializer { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected AsyncLock AsyncLock { get; } = new AsyncLock();

        protected DistributedCacheEntryOptions DefaultCacheOptions;

        private readonly CacheOptions _cacheOption;

        private readonly DistributedCacheOptions _distributedCacheOption;

        public DistributedCache(
            IOptions<CacheOptions> cacheOption,
            IOptions<DistributedCacheOptions> distributedCacheOption,
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,

            IObjectSerializer objectSerializer,
            ICurrentTenant currentTenant)
        {
            _distributedCacheOption = distributedCacheOption.Value;
            _cacheOption = cacheOption.Value;
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
            Logger = NullLogger<DistributedCache<TCacheItem>>.Instance;
            ObjectSerializer = objectSerializer;
            CurrentTenant = currentTenant;

            SetDefaultOptions();
        }

        public virtual TCacheItem Get(string key, bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            byte[] cachedBytes;

            try
            {
                cachedBytes = Cache.Get(NormalizeKey(key));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return null;
                }

                throw;
            }

            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        public virtual async Task<TCacheItem> GetAsync(string key, bool? hideErrors = null, CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            byte[] cachedBytes;

            try
            {
                cachedBytes = await Cache.GetAsync(
                    NormalizeKey(key),
                    CancellationTokenProvider.FallbackToProvider(token)
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return null;
                }

                throw;
            }
            
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        public TCacheItem GetOrAdd(
            string key,
            Func<TCacheItem> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null)
        {
            var value = Get(key, hideErrors);
            if (value != null)
            {
                return value;
            }

            using (AsyncLock.Lock(CancellationTokenProvider.Token))
            {
                value = Get(key, hideErrors);
                if (value != null)
                {
                    return value;
                }

                value = factory();
                Set(key, value, optionsFactory?.Invoke(), hideErrors);
            }

            return value;
        }

        public async Task<TCacheItem> GetOrAddAsync(
            string key,
            Func<Task<TCacheItem>> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null,
            CancellationToken token = default)
        {
            token = CancellationTokenProvider.FallbackToProvider(token);
            var value = await GetAsync(key, hideErrors, token);
            if (value != null)
            {
                return value;
            }

            using (await AsyncLock.LockAsync(token))
            {
                value = await GetAsync(key, hideErrors, token);
                if (value != null)
                {
                    return value;
                }

                value = await factory();
                await SetAsync(key, value, optionsFactory?.Invoke(), hideErrors, token);
            }

            return value;
        }

        public virtual void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null, bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                Cache.Set(
                    NormalizeKey(key),
                    ObjectSerializer.Serialize(value),
                    options ?? DefaultCacheOptions
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return;
                }

                throw;
            }
        }

        public virtual async Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, bool? hideErrors = null, CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await Cache.SetAsync(
                    NormalizeKey(key),
                    ObjectSerializer.Serialize(value),
                    options ?? DefaultCacheOptions,
                    CancellationTokenProvider.FallbackToProvider(token)
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return;
                }

                throw;
            }
        }

        public virtual void Refresh(string key, bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                Cache.Refresh(NormalizeKey(key));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return;
                }

                throw;
            }
        }

        public virtual async Task RefreshAsync(string key, bool? hideErrors = null, CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await Cache.RefreshAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return;
                }

                throw;
            }
        }

        public virtual void Remove(string key, bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                Cache.Remove(NormalizeKey(key));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                }

                throw;
            }
        }

        public virtual async Task RemoveAsync(string key, bool? hideErrors = null, CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await Cache.RemoveAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                    return;
                }

                throw;
            }
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
            CacheName = CacheNameAttribute.GetCacheName(typeof(TCacheItem));

            //IgnoreMultiTenancy
            IgnoreMultiTenancy = typeof(TCacheItem).IsDefined(typeof(IgnoreMultiTenancyAttribute), true);

            //Configure default cache entry options
            DefaultCacheOptions = GetDefaultCacheEntryOptions();
        }
    }
}