using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Threading;

/// <summary>
/// Per-key asynchronous lock for coordinating concurrent flows.
/// </summary>
/// <remarks>
/// Based on the pattern described in https://stackoverflow.com/a/31194647.
/// Use within a <c>using</c> scope to ensure the lock is released via <c>IDisposable.Dispose()</c>.
/// </remarks>
public static class KeyedLock
{
    private static readonly Dictionary<object, RefCounted<SemaphoreSlim>> SemaphoreSlims = new();

    /// <summary>
    /// Acquires an exclusive asynchronous lock for the specified <paramref name="key"/>.
    /// This method waits until the lock becomes available.
    /// </summary>
    /// <param name="key">A non-null object that identifies the lock. Objects considered equal by dictionary semantics will share the same lock.</param>
    /// <returns>An <see cref="IDisposable"/> handle that must be disposed to release the lock.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// var key = "my-critical-section";
    /// using (await KeyedLock.LockAsync(key))
    /// {
    ///     // protected work
    /// }
    /// </code>
    /// </example>
    public static async Task<IDisposable> LockAsync(object key)
    {
        Check.NotNull(key, nameof(key));
        return await LockAsync(key, CancellationToken.None);
    }

    /// <summary>
    /// Acquires an exclusive asynchronous lock for the specified <paramref name="key"/>, observing a <paramref name="cancellationToken"/>.
    /// </summary>
    /// <param name="key">A non-null object that identifies the lock. Objects considered equal by dictionary semantics will share the same lock.</param>
    /// <param name="cancellationToken">A token to cancel the wait for the lock.</param>
    /// <returns>An <see cref="IDisposable"/> handle that must be disposed to release the lock.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the wait is canceled via <paramref name="cancellationToken"/>.</exception>
    /// <example>
    /// <code>
    /// var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    /// using (await KeyedLock.LockAsync("db-update", cts.Token))
    /// {
    ///     // protected work
    /// }
    /// </code>
    /// </example>
    public static async Task<IDisposable> LockAsync(object key, CancellationToken cancellationToken)
    {
        Check.NotNull(key, nameof(key));
        var semaphore = GetOrCreate(key);
        try
        {
            await semaphore.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            var toDispose = DecrementRefAndMaybeRemove(key);
            toDispose?.Dispose();
            throw;
        }
        return new Releaser(key);
    }

    /// <summary>
    /// Attempts to acquire an exclusive lock for the specified <paramref name="key"/> without waiting.
    /// </summary>
    /// <param name="key">A non-null object that identifies the lock.</param>
    /// <returns>
    /// An <see cref="IDisposable"/> handle if the lock was immediately acquired; otherwise <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// var handle = await KeyedLock.TryLockAsync("cache-key");
    /// if (handle != null)
    /// {
    ///     using (handle)
    ///     {
    ///         // protected work
    ///     }
    /// }
    /// </code>
    /// </example>
    public static async Task<IDisposable?> TryLockAsync(object key)
    {
        Check.NotNull(key, nameof(key));
        return await TryLockAsync(key, default, CancellationToken.None);
    }

    /// <summary>
    /// Attempts to acquire an exclusive lock for the specified <paramref name="key"/>, waiting up to <paramref name="timeout"/>.
    /// </summary>
    /// <param name="key">A non-null object that identifies the lock.</param>
    /// <param name="timeout">Maximum time to wait for the lock. If set to <see cref="TimeSpan.Zero"/>, the method performs an immediate, non-blocking attempt.</param>
    /// <param name="cancellationToken">A token to cancel the wait.</param>
    /// <returns>
    /// An <see cref="IDisposable"/> handle if the lock was acquired within the timeout; otherwise <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the wait is canceled via <paramref name="cancellationToken"/>.</exception>
    /// <example>
    /// <code>
    /// var handle = await KeyedLock.TryLockAsync("send-mail", TimeSpan.FromSeconds(1));
    /// if (handle != null)
    /// {
    ///     using (handle)
    ///     {
    ///         // protected work
    ///     }
    /// }
    /// else
    /// {
    ///     // lock not acquired within timeout
    /// }
    /// </code>
    /// </example>
    public static async Task<IDisposable?> TryLockAsync(object key, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        Check.NotNull(key, nameof(key));
        var semaphore = GetOrCreate(key);
        bool acquired;
        try
        {
            if (timeout == default)
            {
                acquired = await semaphore.WaitAsync(0, cancellationToken);
            }
            else
            {
                acquired = await semaphore.WaitAsync(timeout, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            var toDispose = DecrementRefAndMaybeRemove(key);
            toDispose?.Dispose();
            throw;
        }

        if (acquired)
        {
            return new Releaser(key);
        }

        var toDisposeOnFail = DecrementRefAndMaybeRemove(key);
        toDisposeOnFail?.Dispose();

        return null;
    }

    private static SemaphoreSlim GetOrCreate(object key)
    {
        RefCounted<SemaphoreSlim> item;
        lock (SemaphoreSlims)
        {
            if (SemaphoreSlims.TryGetValue(key, out item!))
            {
                ++item.RefCount;
            }
            else
            {
                item = new RefCounted<SemaphoreSlim>(new SemaphoreSlim(1, 1));
                SemaphoreSlims[key] = item;
            }
        }
        return item.Value;
    }

    private sealed class RefCounted<T>(T value)
    {
        public int RefCount { get; set; } = 1;

        public T Value { get; } = value;
    }

    private sealed class Releaser(object key) : IDisposable
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1)
            {
                return;
            }

            RefCounted<SemaphoreSlim> item;
            var shouldDispose = false;
            lock (SemaphoreSlims)
            {
                if (!SemaphoreSlims.TryGetValue(key, out item!))
                {
                    return;
                }
                --item.RefCount;
                if (item.RefCount == 0)
                {
                    SemaphoreSlims.Remove(key);
                    shouldDispose = true;
                }
            }

            if (shouldDispose)
            {
                item.Value.Dispose();
            }
            else
            {
                item.Value.Release();
            }
        }
    }

    private static SemaphoreSlim? DecrementRefAndMaybeRemove(object key)
    {
        RefCounted<SemaphoreSlim>? itemToDispose = null;
        lock (SemaphoreSlims)
        {
            if (SemaphoreSlims.TryGetValue(key, out var item))
            {
                --item.RefCount;
                if (item.RefCount == 0)
                {
                    SemaphoreSlims.Remove(key);
                    itemToDispose = item;
                }
            }
        }
        return itemToDispose?.Value;
    }
}
