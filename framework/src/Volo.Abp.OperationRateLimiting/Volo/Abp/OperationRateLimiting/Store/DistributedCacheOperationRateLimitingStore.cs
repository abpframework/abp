using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Timing;

namespace Volo.Abp.OperationRateLimiting;

public class DistributedCacheOperationRateLimitingStore : IOperationRateLimitingStore, ITransientDependency
{
    protected IDistributedCache<OperationRateLimitingCacheItem> Cache { get; }
    protected IClock Clock { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpOperationRateLimitingOptions Options { get; }

    public DistributedCacheOperationRateLimitingStore(
        IDistributedCache<OperationRateLimitingCacheItem> cache,
        IClock clock,
        IAbpDistributedLock distributedLock,
        IOptions<AbpOperationRateLimitingOptions> options)
    {
        Cache = cache;
        Clock = clock;
        DistributedLock = distributedLock;
        Options = options.Value;
    }

    public virtual async Task<OperationRateLimitingStoreResult> IncrementAsync(
        string key, TimeSpan duration, int maxCount)
    {
        if (maxCount <= 0)
        {
            return new OperationRateLimitingStoreResult
            {
                IsAllowed = false,
                CurrentCount = 0,
                MaxCount = maxCount,
                RetryAfter = duration
            };
        }

        await using (var handle = await DistributedLock.TryAcquireAsync(
            $"OperationRateLimiting:{key}", Options.LockTimeout))
        {
            if (handle == null)
            {
                throw new AbpException(
                    "Could not acquire distributed lock for operation rate limit. " +
                    "This is an infrastructure issue, not a rate limit violation.");
            }

            var cacheItem = await Cache.GetAsync(key);
            var now = new DateTimeOffset(Clock.Now.ToUniversalTime());

            if (cacheItem == null || now >= cacheItem.WindowStart.Add(duration))
            {
                cacheItem = new OperationRateLimitingCacheItem { Count = 1, WindowStart = now };
                await Cache.SetAsync(key, cacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = duration
                    });

                return new OperationRateLimitingStoreResult
                {
                    IsAllowed = true,
                    CurrentCount = 1,
                    MaxCount = maxCount
                };
            }

            if (cacheItem.Count >= maxCount)
            {
                var retryAfter = cacheItem.WindowStart.Add(duration) - now;
                return new OperationRateLimitingStoreResult
                {
                    IsAllowed = false,
                    CurrentCount = cacheItem.Count,
                    MaxCount = maxCount,
                    RetryAfter = retryAfter
                };
            }

            cacheItem.Count++;
            var expiration = cacheItem.WindowStart.Add(duration) - now;
            await Cache.SetAsync(key, cacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration > TimeSpan.Zero ? expiration : duration
                });

            return new OperationRateLimitingStoreResult
            {
                IsAllowed = true,
                CurrentCount = cacheItem.Count,
                MaxCount = maxCount
            };
        }
    }

    public virtual async Task<OperationRateLimitingStoreResult> GetAsync(
        string key, TimeSpan duration, int maxCount)
    {
        if (maxCount <= 0)
        {
            return new OperationRateLimitingStoreResult
            {
                IsAllowed = false,
                CurrentCount = 0,
                MaxCount = maxCount,
                RetryAfter = duration
            };
        }

        var cacheItem = await Cache.GetAsync(key);
        var now = new DateTimeOffset(Clock.Now.ToUniversalTime());

        if (cacheItem == null || now >= cacheItem.WindowStart.Add(duration))
        {
            return new OperationRateLimitingStoreResult
            {
                IsAllowed = true,
                CurrentCount = 0,
                MaxCount = maxCount
            };
        }

        if (cacheItem.Count >= maxCount)
        {
            var retryAfter = cacheItem.WindowStart.Add(duration) - now;
            return new OperationRateLimitingStoreResult
            {
                IsAllowed = false,
                CurrentCount = cacheItem.Count,
                MaxCount = maxCount,
                RetryAfter = retryAfter
            };
        }

        return new OperationRateLimitingStoreResult
        {
            IsAllowed = true,
            CurrentCount = cacheItem.Count,
            MaxCount = maxCount
        };
    }

    public virtual async Task ResetAsync(string key)
    {
        await Cache.RemoveAsync(key);
    }
}
