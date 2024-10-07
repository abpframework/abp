using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Hybrid;

namespace Volo.Abp.Caching.Hybrid;

/// <summary>
/// Represents a hybrid cache of <typeparamref name="TCacheItem"/> items.
/// </summary>
/// <typeparam name="TCacheItem">The type of the cache item being cached.</typeparam>
public interface IHybridCache<TCacheItem> : IHybridCache<TCacheItem, string>
    where TCacheItem : class
{
    IHybridCache<TCacheItem, string> InternalCache { get; }
}

/// <summary>
/// Represents a hybrid cache of <typeparamref name="TCacheItem"/> items.
/// Uses <typeparamref name="TCacheKey"/> as the key type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
/// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
public interface IHybridCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
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
    Task<TCacheItem?> GetOrCreateAsync(
        [NotNull]TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<HybridCacheEntryOptions>? optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default);

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
        [NotNull]TCacheKey key,
        TCacheItem value,
        HybridCacheEntryOptions? options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default);

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    Task RemoveAsync(
        [NotNull]TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default);

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
        CancellationToken token = default);
}
