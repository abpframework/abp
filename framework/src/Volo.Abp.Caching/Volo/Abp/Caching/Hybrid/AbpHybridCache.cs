using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.Caching.Hybrid;

/// <summary>
/// Represents a hybrid cache of <typeparamref name="TCacheItem"/> items.
/// </summary>
/// <typeparam name="TCacheItem">The type of the cache item being cached.</typeparam>
public class AbpHybridCache<TCacheItem> : IHybridCache<TCacheItem>
    where TCacheItem : class
{
    public IHybridCache<TCacheItem, string> InternalCache { get; }

    public AbpHybridCache(IHybridCache<TCacheItem, string> internalCache)
    {
        InternalCache = internalCache;
    }

    public virtual async Task<TCacheItem?> GetOrCreateAsync(string key, Func<Task<TCacheItem>> factory, Func<HybridCacheEntryOptions>? optionsFactory = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        return await InternalCache.GetOrCreateAsync(key, factory, optionsFactory, hideErrors, considerUow, token);
    }

    public virtual async Task SetAsync(string key, TCacheItem value, HybridCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        await InternalCache.SetAsync(key, value, options, hideErrors, considerUow, token);
    }

    public virtual async Task RemoveAsync(string key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        await InternalCache.RemoveAsync(key, hideErrors, considerUow, token);
    }

    public virtual async Task RemoveManyAsync(IEnumerable<string> keys, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        await InternalCache.RemoveManyAsync(keys, hideErrors, considerUow, token);
    }
}

/// <summary>
/// Represents a hybrid cache of <typeparamref name="TCacheItem"/> items.
/// Uses <typeparamref name="TCacheKey"/> as the key type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
/// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
public class AbpHybridCache<TCacheItem, TCacheKey> : IHybridCache<TCacheItem, TCacheKey>
    where TCacheItem : class
    where TCacheKey : notnull
{
    public const string UowCacheName = "AbpHybridCache";

    public ILogger<AbpHybridCache<TCacheItem, TCacheKey>> Logger { get; set; }

    protected string CacheName { get; set; } = default!;

    protected bool IgnoreMultiTenancy { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    protected HybridCache HybridCache { get; }

    protected IDistributedCache DistributedCacheCache { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected IDistributedCacheKeyNormalizer KeyNormalizer { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected SemaphoreSlim SyncSemaphore { get; }

    protected HybridCacheEntryOptions DefaultCacheOptions = default!;

    protected AbpHybridCacheOptions DistributedCacheOption { get; }

    public AbpHybridCache(
        IServiceProvider serviceProvider,
        IOptions<AbpHybridCacheOptions> distributedCacheOption,
        HybridCache hybridCache,
        IDistributedCache distributedCache,
        ICancellationTokenProvider cancellationTokenProvider,
        IDistributedCacheSerializer serializer,
        IDistributedCacheKeyNormalizer keyNormalizer,
        IServiceScopeFactory serviceScopeFactory,
        IUnitOfWorkManager unitOfWorkManager)
    {
        ServiceProvider = serviceProvider;
        DistributedCacheOption = distributedCacheOption.Value;
        HybridCache = hybridCache;
        DistributedCacheCache = distributedCache;
        CancellationTokenProvider = cancellationTokenProvider;
        Logger = NullLogger<AbpHybridCache<TCacheItem, TCacheKey>>.Instance;
        KeyNormalizer = keyNormalizer;
        ServiceScopeFactory = serviceScopeFactory;
        UnitOfWorkManager = unitOfWorkManager;

        SyncSemaphore = new SemaphoreSlim(1, 1);

        SetDefaultOptions();
    }

    protected virtual string NormalizeKey(TCacheKey key)
    {
        return KeyNormalizer.NormalizeKey(
            new DistributedCacheKeyNormalizeArgs(
                key.ToString()!,
                CacheName,
                IgnoreMultiTenancy
            )
        );
    }

    protected virtual HybridCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        foreach (var configure in DistributedCacheOption.CacheConfigurators)
        {
            var options = configure.Invoke(CacheName);
            if (options != null)
            {
                return options;
            }
        }

        return DistributedCacheOption.GlobalHybridCacheEntryOptions;
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
    /// Gets or Creates a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item.</returns>
    public virtual async Task<TCacheItem?> GetOrCreateAsync(
        TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<HybridCacheEntryOptions>? optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        token = CancellationTokenProvider.FallbackToProvider(token);
        hideErrors ??= DistributedCacheOption.HideErrors;

        TCacheItem? value = null;

        if (!considerUow)
        {
            try
            {
                value = await HybridCache.GetOrCreateAsync(
                    key: NormalizeKey(key),
                    factory: async cancel => await factory(),
                    options: optionsFactory?.Invoke(),
                    tags: null,
                    cancellationToken: token);
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

            return value;
        }

        try
        {
            using (await SyncSemaphore.LockAsync(token))
            {
                if (ShouldConsiderUow(considerUow))
                {
                    value = GetUnitOfWorkCache().GetOrDefault(key)?.GetUnRemovedValueOrNull();
                    if (value != null)
                    {
                        return value;
                    }
                }

                var bytes = await DistributedCacheCache.GetAsync(NormalizeKey(key), token);
                if (bytes != null)
                {
                    return ResolveSerializer().Deserialize(new ReadOnlySequence<byte>(bytes, 0, bytes.Length));;
                }

                value = await factory();

                if (ShouldConsiderUow(considerUow))
                {
                    var uowCache = GetUnitOfWorkCache();
                    if (uowCache.TryGetValue(key, out var item))
                    {
                        item.SetValue(value);
                    }
                    else
                    {
                        uowCache.Add(key, new UnitOfWorkCacheItem<TCacheItem>(value));
                    }
                }

                await SetAsync(key, value, optionsFactory?.Invoke(), hideErrors, considerUow, token);
            }
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

        return value;
    }

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public virtual async Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        HybridCacheEntryOptions? options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        async Task SetRealCache()
        {
            token = CancellationTokenProvider.FallbackToProvider(token);
            hideErrors ??= DistributedCacheOption.HideErrors;

            try
            {
                await HybridCache.SetAsync(
                    key: NormalizeKey(key),
                    value: value,
                    options: options ?? DefaultCacheOptions,
                    tags: null,
                    cancellationToken: token
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

        if (ShouldConsiderUow(considerUow))
        {
            var uowCache = GetUnitOfWorkCache();
            if (uowCache.TryGetValue(key, out _))
            {
                uowCache[key].SetValue(value);
            }
            else
            {
                uowCache.Add(key, new UnitOfWorkCacheItem<TCacheItem>(value));
            }

            UnitOfWorkManager.Current?.OnCompleted(SetRealCache);
        }
        else
        {
            await SetRealCache();
        }
    }

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public virtual async Task RemoveAsync(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        await RemoveManyAsync(new[] { key }, hideErrors, considerUow, token);
    }

    /// <summary>
    /// Removes the cache items for given keys from cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public async Task RemoveManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        var keyArray = keys.ToArray();

        async Task RemoveRealCache()
        {
            hideErrors ??= DistributedCacheOption.HideErrors;

            try
            {
                await HybridCache.RemoveAsync(
                    keyArray.Select(NormalizeKey), token);
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

        if (ShouldConsiderUow(considerUow))
        {
            var uowCache = GetUnitOfWorkCache();

            foreach (var key in keyArray)
            {
                if (uowCache.TryGetValue(key, out _))
                {
                    uowCache[key].RemoveValue();
                }
            }

            UnitOfWorkManager.Current?.OnCompleted(RemoveRealCache);
        }
        else
        {
            await RemoveRealCache();
        }
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

    protected virtual bool ShouldConsiderUow(bool considerUow)
    {
        return considerUow && UnitOfWorkManager.Current != null;
    }

    protected virtual string GetUnitOfWorkCacheKey()
    {
        return UowCacheName + CacheName;
    }

    protected virtual Dictionary<TCacheKey, UnitOfWorkCacheItem<TCacheItem>> GetUnitOfWorkCache()
    {
        if (UnitOfWorkManager.Current == null)
        {
            throw new AbpException($"There is no active UOW.");
        }

        return UnitOfWorkManager.Current.GetOrAddItem(GetUnitOfWorkCacheKey(),
            key => new Dictionary<TCacheKey, UnitOfWorkCacheItem<TCacheItem>>());
    }

    private readonly ConcurrentDictionary<Type, object> _serializersCache = new();

    protected virtual IHybridCacheSerializer<TCacheItem> ResolveSerializer()
    {
        if (_serializersCache.TryGetValue(typeof(TCacheItem), out var serializer))
        {
            return serializer.As<IHybridCacheSerializer<TCacheItem>>();
        }

        serializer = ServiceProvider.GetService<IHybridCacheSerializer<TCacheItem>>();
        if (serializer is null)
        {
            var factories = ServiceProvider.GetServices<IHybridCacheSerializerFactory>().ToArray();
            Array.Reverse(factories);
            foreach (var factory in factories)
            {
                if (factory.TryCreateSerializer<TCacheItem>(out var current))
                {
                    serializer = current;
                    break;
                }
            }
        }

        if (serializer is null)
        {
            throw new InvalidOperationException($"No {nameof(IHybridCacheSerializer<TCacheItem>)} configured for type '{typeof(TCacheItem).Name}'");
        }

        return serializer.As<IHybridCacheSerializer<TCacheItem>>();
    }
}
