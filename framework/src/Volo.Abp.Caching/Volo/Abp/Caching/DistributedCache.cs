using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
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
            IDistributedCacheKeyNormalizer keyNormalizer,
            IHybridServiceScopeFactory serviceScopeFactory) : base(
            distributedCacheOption: distributedCacheOption,
            cache: cache,
            cancellationTokenProvider: cancellationTokenProvider,
            serializer: serializer,
            keyNormalizer: keyNormalizer,
            serviceScopeFactory: serviceScopeFactory)
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

        protected IDistributedCacheKeyNormalizer KeyNormalizer { get; }

        protected IHybridServiceScopeFactory ServiceScopeFactory { get; }

        protected SemaphoreSlim SyncSemaphore { get; }

        protected DistributedCacheEntryOptions DefaultCacheOptions;

        private readonly AbpDistributedCacheOptions _distributedCacheOption;

        public DistributedCache(
            IOptions<AbpDistributedCacheOptions> distributedCacheOption,
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IDistributedCacheSerializer serializer,
            IDistributedCacheKeyNormalizer keyNormalizer,
            IHybridServiceScopeFactory serviceScopeFactory)
        {
            _distributedCacheOption = distributedCacheOption.Value;
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
            Logger = NullLogger<DistributedCache<TCacheItem, TCacheKey>>.Instance;
            Serializer = serializer;
            KeyNormalizer = keyNormalizer;
            ServiceScopeFactory = serviceScopeFactory;

            SyncSemaphore = new SemaphoreSlim(1, 1);

            SetDefaultOptions();
        }

        protected virtual string NormalizeKey(TCacheKey key)
        {
            return KeyNormalizer.NormalizeKey(
                new DistributedCacheKeyNormalizeArgs(
                    key.ToString(),
                    CacheName,
                    IgnoreMultiTenancy
                )
            );
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
                    HandleException(ex);
                    return null;
                }

                throw;
            }

            return ToCacheItem(cachedBytes);
        }

        public virtual KeyValuePair<TCacheKey, TCacheItem>[] GetMany(
            IEnumerable<TCacheKey> keys,
            bool? hideErrors = null)
        {
            var keyArray = keys.ToArray();

            var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
            if (cacheSupportsMultipleItems == null)
            {
                return GetManyFallback(
                    keyArray,
                    hideErrors
                );
            }

            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
            byte[][] cachedBytes;

            try
            {
                cachedBytes = cacheSupportsMultipleItems.GetMany(keyArray.Select(NormalizeKey));
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return ToCacheItemsWithDefaultValues(keyArray);
                }

                throw;
            }
            
            return ToCacheItems(cachedBytes, keyArray);
        }

        protected virtual KeyValuePair<TCacheKey, TCacheItem>[] GetManyFallback(
            TCacheKey[] keys,
            bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                return keys
                    .Select(key => new KeyValuePair<TCacheKey, TCacheItem>(
                            key,
                            Get(key, hideErrors: false)
                        )
                    ).ToArray();
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return ToCacheItemsWithDefaultValues(keys);
                }

                throw;
            }
        }

        public virtual async Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetManyAsync(
            IEnumerable<TCacheKey> keys,
            bool? hideErrors = null,
            CancellationToken token = default)
        {
            var keyArray = keys.ToArray();

            var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
            if (cacheSupportsMultipleItems == null)
            {
                return await GetManyFallbackAsync(
                    keyArray,
                    hideErrors,
                    token
                );
            }

            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
            byte[][] cachedBytes;

            try
            {
                cachedBytes = await cacheSupportsMultipleItems.GetManyAsync(
                    keyArray.Select(NormalizeKey),
                    CancellationTokenProvider.FallbackToProvider(token)
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return ToCacheItemsWithDefaultValues(keyArray);
                }

                throw;
            }
            
            return ToCacheItems(cachedBytes, keyArray);
        }

        protected virtual async Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetManyFallbackAsync(
            TCacheKey[] keys,
            bool? hideErrors = null,
            CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                var result = new List<KeyValuePair<TCacheKey, TCacheItem>>();

                foreach (var key in keys)
                {
                    result.Add(new KeyValuePair<TCacheKey, TCacheItem>(
                        key,
                        await GetAsync(key, false, token))
                    );
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return ToCacheItemsWithDefaultValues(keys);
                }

                throw;
            }
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
                    await HandleExceptionAsync(ex);
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
                    HandleException(ex);
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
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }

        public void SetMany(
            IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null)
        {
            var itemsArray = items.ToArray();

            var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
            if (cacheSupportsMultipleItems == null)
            {
                SetManyFallback(
                    itemsArray,
                    options,
                    hideErrors
                );
                
                return;
            }
            
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                cacheSupportsMultipleItems.SetMany(
                    ToRawCacheItems(itemsArray),
                    options ?? DefaultCacheOptions
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }
        
        protected virtual void SetManyFallback(
            KeyValuePair<TCacheKey, TCacheItem>[] items,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;
            
            try
            {
                foreach (var item in items)
                {
                    Set(
                        item.Key,
                        item.Value,
                        options: options,
                        hideErrors: false
                    );
                }
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }

        public virtual async Task SetManyAsync(
            IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null,
            CancellationToken token = default)
        {
            var itemsArray = items.ToArray();

            var cacheSupportsMultipleItems = Cache as ICacheSupportsMultipleItems;
            if (cacheSupportsMultipleItems == null)
            {
                await SetManyFallbackAsync(
                    itemsArray,
                    options,
                    hideErrors,
                    token
                );
                
                return;
            }
            
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                await cacheSupportsMultipleItems.SetManyAsync(
                    ToRawCacheItems(itemsArray),
                    options ?? DefaultCacheOptions,
                    CancellationTokenProvider.FallbackToProvider(token)
                );
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }
        
        protected virtual async Task SetManyFallbackAsync(
            KeyValuePair<TCacheKey, TCacheItem>[] items,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null,
            CancellationToken token = default)
        {
            hideErrors = hideErrors ?? _distributedCacheOption.HideErrors;

            try
            {
                foreach (var item in items)
                {
                    await SetAsync(
                        item.Key,
                        item.Value,
                        options: options,
                        hideErrors: false,
                        token: token
                    );
                }
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }

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
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }

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
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }

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
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }

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
                    await HandleExceptionAsync(ex);
                    return;
                }

                throw;
            }
        }

        protected virtual void HandleException(Exception ex)
        {
            AsyncHelper.RunSync(() => HandleExceptionAsync(ex));
        }
        
        protected virtual async Task HandleExceptionAsync(Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex, LogLevel.Warning));
            }
        }
        
        protected virtual KeyValuePair<TCacheKey, TCacheItem>[] ToCacheItems(byte[][] itemBytes, TCacheKey[] itemKeys)
        {
            if (itemBytes.Length != itemKeys.Length)
            {
                throw new AbpException("count of the item bytes should be same with the count of the given keys");
            }

            var result = new List<KeyValuePair<TCacheKey, TCacheItem>>();

            for (int i = 0; i < itemKeys.Length; i++)
            {
                result.Add(
                    new KeyValuePair<TCacheKey, TCacheItem>(
                        itemKeys[i],
                        ToCacheItem(itemBytes[i])
                    )
                );
            }

            return result.ToArray();
        }
        
        [CanBeNull]
        protected virtual TCacheItem ToCacheItem([CanBeNull] byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }

            return Serializer.Deserialize<TCacheItem>(bytes);
        }


        protected virtual KeyValuePair<string, byte[]>[] ToRawCacheItems(KeyValuePair<TCacheKey, TCacheItem>[] items)
        {
            return items
                .Select(i => new KeyValuePair<string, byte[]>(
                        NormalizeKey(i.Key),
                        Serializer.Serialize(i.Value)
                    )
                ).ToArray();
        }
        
        private static KeyValuePair<TCacheKey, TCacheItem>[] ToCacheItemsWithDefaultValues(TCacheKey[] keys)
        {
            return keys
                .Select(key => new KeyValuePair<TCacheKey, TCacheItem>(key, default))
                .ToArray();
        }
    }
}