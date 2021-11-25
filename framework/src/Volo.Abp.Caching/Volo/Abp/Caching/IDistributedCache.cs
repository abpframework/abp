using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;

namespace Volo.Abp.Caching;

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
public interface IDistributedCache<TCacheItem> : IDistributedCache<TCacheItem, string>
    where TCacheItem : class
{
}

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// Uses a generic cache key type of <typeparamref name="TCacheKey" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
/// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
public interface IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    /// <summary>
    /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <returns>The cache item, or null.</returns>
    TCacheItem Get(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Gets multiple cache items with the given keys.
    ///
    /// The returned list contains exactly the same count of items specified in the given keys.
    /// An item in the return list can not be null, but an item in the list has null value
    /// if the related key not found in the cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <returns>List of cache items.</returns>
    KeyValuePair<TCacheKey, TCacheItem>[] GetMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Gets multiple cache items with the given keys.
    ///
    /// The returned list contains exactly the same count of items specified in the given keys.
    /// An item in the return list can not be null, but an item in the list has null value
    /// if the related key not found in the cache.
    ///
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>List of cache items.</returns>
    Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item, or null.</returns>
    Task<TCacheItem> GetAsync(
        [NotNull] TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <returns>The cache item.</returns>
    TCacheItem GetOrAdd(
        TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item.</returns>
    Task<TCacheItem> GetOrAddAsync(
        [NotNull] TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Gets or Adds multiple cache items with the given keys. If any cache items not found for the given keys then adds cache items
    /// provided by <paramref name="factory" /> delegate and returns the provided cache items.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache items when no cache items are found for the given <paramref name="keys" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <returns>The cache items.</returns>
    KeyValuePair<TCacheKey, TCacheItem>[] GetOrAddMany(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, List<KeyValuePair<TCacheKey, TCacheItem>>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Gets or Adds multiple cache items with the given keys. If any cache items not found for the given keys then adds cache items
    /// provided by <paramref name="factory" /> delegate and returns the provided cache items.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache items when no cache items are found for the given <paramref name="keys" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache items.</returns>
    Task<KeyValuePair<TCacheKey, TCacheItem>[]> GetOrAddManyAsync(
        IEnumerable<TCacheKey> keys,
        Func<IEnumerable<TCacheKey>, Task<List<KeyValuePair<TCacheKey, TCacheItem>>>> factory,
        Func<DistributedCacheEntryOptions> optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        bool considerUow = false
    );

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
    Task SetAsync(
        [NotNull] TCacheKey key,
        [NotNull] TCacheItem value,
        [CanBeNull] DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Sets multiple cache items.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="items">Items to set on the cache</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    void SetMany(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Sets multiple cache items.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="items">Items to set on the cache</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task SetManyAsync(
        IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items,
        DistributedCacheEntryOptions options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    void Refresh(
        TCacheKey key,
        bool? hideErrors = null
    );

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RefreshAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default
    );

    /// <summary>
    /// Refreshes the cache value of the given keys, and resets their sliding expiration timeout.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    void RefreshMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null);

    /// <summary>
    /// Refreshes the cache value of the given keys, and resets their sliding expiration timeout.
    /// Based on the implementation, this can be more efficient than setting multiple items individually.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RefreshManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        CancellationToken token = default);

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    void Remove(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RemoveAsync(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );

    /// <summary>
    /// Removes the cache items for given keys from cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    void RemoveMany(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        bool considerUow = false
    );

    /// <summary>
    /// Removes the cache items for given keys from cache.
    /// </summary>
    /// <param name="keys">The keys of cached items to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RemoveManyAsync(
        IEnumerable<TCacheKey> keys,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default
    );
}
