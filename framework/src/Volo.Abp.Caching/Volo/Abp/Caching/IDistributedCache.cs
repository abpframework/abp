using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;

namespace Volo.Abp.Caching
{
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
        /// <returns>The cache item, or null.</returns>
        TCacheItem Get(
            TCacheKey key,
            bool? hideErrors = null
        );

        /// <summary>
        /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The cache item, or null.</returns>
        Task<TCacheItem> GetAsync(
            [NotNull] TCacheKey key,
            bool? hideErrors = null,
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
        /// <returns>The cache item.</returns>
        TCacheItem GetOrAdd(
            TCacheKey key,
            Func<TCacheItem> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null
        );

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
        Task<TCacheItem> GetOrAddAsync(
            [NotNull] TCacheKey key,
            Func<Task<TCacheItem>> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null,
            CancellationToken token = default
        );

        /// <summary>
        /// Sets the cache item value for the provided key.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="value">The cache item value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        void Set(
            TCacheKey key,
            TCacheItem value,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null
        );

        /// <summary>
        /// Sets the cache item value for the provided key.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="value">The cache item value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
        Task SetAsync(
            [NotNull] TCacheKey key,
            [NotNull] TCacheItem value,
            [CanBeNull] DistributedCacheEntryOptions options = null,
            bool? hideErrors = null,
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
        /// Removes the cache item for given key from cache.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        void Remove(
            TCacheKey key,
            bool? hideErrors = null
        );

        /// <summary>
        /// Removes the cache item for given key from cache.
        /// </summary>
        /// <param name="key">The key of cached item to be retrieved from the cache.</param>
        /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
        /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
        Task RemoveAsync(
            TCacheKey key,
            bool? hideErrors = null,
            CancellationToken token = default
        );
    }
}
