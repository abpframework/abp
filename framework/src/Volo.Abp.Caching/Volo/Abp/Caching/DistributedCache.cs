using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Caching
{
    /// <summary>
    /// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
    public class DistributedCache<TCacheItem> : DistributedCache<TCacheItem, string>, IDistributedCache<TCacheItem>
        where TCacheItem : class
    {
        public DistributedCache(
            IOptions<AbpDistributedCacheOptions> distributedCacheOption,
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IDistributedCacheSerializer serializer,
            ICurrentTenant currentTenant) : base(
                distributedCacheOption: distributedCacheOption,
                cache: cache,
                cancellationTokenProvider: cancellationTokenProvider,
                serializer: serializer,
                currentTenant: currentTenant)
        {
        }

    }
    /// <summary>
    /// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
    /// Uses a generic cache key type of <typeparamref name="TCacheKey" /> type.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
    /// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
    public class DistributedCache<TCacheItem, TCacheKey> : IDistributedCache<TCacheItem, TCacheKey>
        where TCacheItem : class
    {
        public ILogger<DistributedCache<TCacheItem, TCacheKey>> Logger { get; set; }

        protected string CacheName { get; set; }

        protected bool IgnoreMultiTenancy { get; set; }

        protected IDistributedCache Cache { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        protected IDistributedCacheSerializer Serializer { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected SemaphoreSlim SyncSemaphore { get; }

        protected DistributedCacheEntryOptions DefaultCacheOptions;

        private readonly AbpDistributedCacheOptions _distributedCacheOption;

        public DistributedCache(
            IOptions<AbpDistributedCacheOptions> distributedCacheOption,
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IDistributedCacheSerializer serializer,
            ICurrentTenant currentTenant)
        {
            _distributedCacheOption = distributedCacheOption.Value;
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
            Logger = NullLogger<DistributedCache<TCacheItem, TCacheKey>>.Instance;
            Serializer = serializer;
            CurrentTenant = currentTenant;

            SyncSemaphore = new SemaphoreSlim(1, 1);

            SetDefaultOptions();
        }
        protected virtual string NormalizeKey(TCacheKey key)
        {
            var normalizedKey = "c:" + CacheName + ",k:" + _distributedCacheOption.KeyPrefix + key.ToString();

            if (!IgnoreMultiTenancy && CurrentTenant.Id.HasValue)
            {
                normalizedKey = "t:" + CurrentTenant.Id.Value + "," + normalizedKey;
            }

            return normalizedKey;
        }

        protected virtual DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
        {
            foreach (var configure in _distributedCacheOption.CacheConfigurators)
            {
                var options = configure.Invoke(CacheName);
                if (options != null)
                {
                    return options;
                }
            }
            return _distributedCacheOption.GlobalCacheEntryOptions;
        }

        protected virtual void SetDefaultOptions()
        {
            CacheName = CacheNameAttribute.GetCacheName(typeof(TCacheItem));

            //IgnoreMultiTenancy
            IgnoreMultiTenancy = typeof(TCacheItem).IsDefined(typeof(IgnoreMultiTenancyAttribute), true);

            //Configure default cache entry options
            DefaultCacheOptions = GetDefaultCacheEntryOptions();
        }
        /// <summary>
        /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <returns>The cache item, or null.</returns>
        public virtual TCacheItem Get(
            TCacheKey key,
            bool? hideErrors = null)
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

            return Serializer.Deserialize<TCacheItem>(cachedBytes);
        }

        /// <summary>
        /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The cache item, or null.</returns>
        public virtual async Task<TCacheItem> GetAsync(
            TCacheKey key,
            bool? hideErrors = null,
            CancellationToken token = default)
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

            return Serializer.Deserialize<TCacheItem>(cachedBytes);
        }
        /// <summary>
        /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
        /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
        /// <param name="optionsFactory">The cache options for the factory delegate.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <returns>The cache item.</returns>
        public virtual TCacheItem GetOrAdd(
            TCacheKey key,
            Func<TCacheItem> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null)
        {
            var value = Get(key, hideErrors);
            if (value != null)
            {
                return value;
            }

            using (SyncSemaphore.Lock())
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
        /// <summary>
        /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
        /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
        /// <param name="optionsFactory">The cache options for the factory delegate.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The cache item.</returns>
        public virtual async Task<TCacheItem> GetOrAddAsync(
            TCacheKey key,
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

            using (await SyncSemaphore.LockAsync(token))
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
        /// <summary>
        /// Sets the cache item value for the provided key.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="value">The cache item value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        public virtual void Set(
            TCacheKey key,
            TCacheItem value,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                Cache.Set(
                    NormalizeKey(key),
                    Serializer.Serialize(value),
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
        /// <summary>
        /// Sets the cache item value for the provided key.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="value">The cache item value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
        public virtual async Task SetAsync(
            TCacheKey key,
            TCacheItem value,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null,
            CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await Cache.SetAsync(
                    NormalizeKey(key),
                    Serializer.Serialize(value),
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
        /// <summary>
        /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        public virtual void Refresh(
            TCacheKey key, bool?
            hideErrors = null)
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
        /// <summary>
        /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
        public virtual async Task RefreshAsync(
            TCacheKey key,
            bool? hideErrors = null,
            CancellationToken token = default)
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
        /// <summary>
        /// Removes the cache item for given key from cache.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        public virtual void Remove(
            TCacheKey key,
            bool? hideErrors = null)
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
        /// <summary>
        /// Removes the cache item for given key from cache.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
        public virtual async Task RemoveAsync(
            TCacheKey key,
            bool? hideErrors = null,
            CancellationToken token = default)
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

    }

}