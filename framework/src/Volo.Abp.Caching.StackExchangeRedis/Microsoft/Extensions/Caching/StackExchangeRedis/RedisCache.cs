// This software is part of the DOTNET extensions
// Copyright (c) .NET Foundation and Contributors
// https://dotnet.microsoft.com/
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Microsoft.Extensions.Caching.StackExchangeRedis
{
    public class RedisCache : IDistributedCache, IDisposable
    {
        // KEYS[1] = = key
        // ARGV[1] = absolute-expiration - ticks as long (-1 for none)
        // ARGV[2] = sliding-expiration - ticks as long (-1 for none)
        // ARGV[3] = relative-expiration (long, in seconds, -1 for none) - Min(absolute-expiration - Now, sliding-expiration)
        // ARGV[4] = data - byte[]
        // this order should not change LUA script depends on it
        protected const string SetScript = (@"
                redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
                if ARGV[3] ~= '-1' then
                  redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1");

        protected const string AbsoluteExpirationKey = "absexp";
        protected const string SlidingExpirationKey = "sldexp";
        protected const string DataKey = "data";
        protected const long NotPresent = -1;

        protected volatile ConnectionMultiplexer Connection;
        protected IDatabase Cache;

        protected readonly RedisCacheOptions Options;
        protected readonly string Instance;

        protected readonly SemaphoreSlim ConnectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public RedisCache(IOptions<RedisCacheOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            Options = optionsAccessor.Value;

            // This allows partitioning a single backend cache for use with multiple apps/services.
            Instance = Options.InstanceName ?? string.Empty;
        }

        public virtual byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return GetAndRefresh(key, getData: true);
        }

        public virtual async Task<byte[]> GetAsync(
            string key,
            CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            return await GetAndRefreshAsync(key, getData: true, token: token);
        }

        public virtual void Set(
            string key,
            byte[] value,
            DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Connect();

            var creationTime = DateTimeOffset.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

            Cache.ScriptEvaluate(SetScript, new RedisKey[] {Instance + key},
                new RedisValue[]
                {
                    absoluteExpiration?.Ticks ?? NotPresent,
                    options.SlidingExpiration?.Ticks ?? NotPresent,
                    GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                    value
                });
        }

        public virtual async Task SetAsync(
            string key,
            byte[] value,
            DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            var creationTime = DateTimeOffset.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

            await Cache.ScriptEvaluateAsync(SetScript, new RedisKey[] {Instance + key},
                new RedisValue[]
                {
                    absoluteExpiration?.Ticks ?? NotPresent,
                    options.SlidingExpiration?.Ticks ?? NotPresent,
                    GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                    value
                });
        }

        public virtual void Refresh(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            GetAndRefresh(key, getData: false);
        }

        public virtual async Task RefreshAsync(
            string key,
            CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            await GetAndRefreshAsync(key, getData: false, token: token);
        }

        protected virtual void Connect()
        {
            if (Cache != null)
            {
                return;
            }

            ConnectionLock.Wait();
            try
            {
                if (Cache == null)
                {
                    if (Options.ConfigurationOptions != null)
                    {
                        Connection = ConnectionMultiplexer.Connect(Options.ConfigurationOptions);
                    }
                    else
                    {
                        Connection = ConnectionMultiplexer.Connect(Options.Configuration);
                    }

                    Cache = Connection.GetDatabase();
                }
            }
            finally
            {
                ConnectionLock.Release();
            }
        }

        protected virtual async Task ConnectAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (Cache != null)
            {
                return;
            }

            await ConnectionLock.WaitAsync(token);
            try
            {
                if (Cache == null)
                {
                    if (Options.ConfigurationOptions != null)
                    {
                        Connection = await ConnectionMultiplexer.ConnectAsync(Options.ConfigurationOptions);
                    }
                    else
                    {
                        Connection = await ConnectionMultiplexer.ConnectAsync(Options.Configuration);
                    }

                    Cache = Connection.GetDatabase();
                }
            }
            finally
            {
                ConnectionLock.Release();
            }
        }

        protected virtual byte[] GetAndRefresh(string key, bool getData)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Connect();

            // This also resets the LRU status as desired.
            // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
            RedisValue[] results;
            if (getData)
            {
                results = Cache.HashMemberGet(Instance + key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
            }
            else
            {
                results = Cache.HashMemberGet(Instance + key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

            // TODO: Error handling
            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                Refresh(key, absExpr, sldExpr);
            }

            if (results.Length >= 3 && results[2].HasValue)
            {
                return results[2];
            }

            return null;
        }

        protected virtual async Task<byte[]> GetAndRefreshAsync(
            string key,
            bool getData,
            CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            // This also resets the LRU status as desired.
            // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
            RedisValue[] results;
            if (getData)
            {
                results = await Cache.HashMemberGetAsync(Instance + key, AbsoluteExpirationKey, SlidingExpirationKey,
                    DataKey);
            }
            else
            {
                results = await Cache.HashMemberGetAsync(Instance + key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

            // TODO: Error handling
            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                await RefreshAsync(key, absExpr, sldExpr, token);
            }

            if (results.Length >= 3 && results[2].HasValue)
            {
                return results[2];
            }

            return null;
        }

        public virtual void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Connect();

            Cache.KeyDelete(Instance + key);
            // TODO: Error handling
        }

        public virtual async Task RemoveAsync(
            string key,
            CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await ConnectAsync(token);

            await Cache.KeyDeleteAsync(Instance + key);
            // TODO: Error handling
        }

        protected virtual void MapMetadata(
            RedisValue[] results,
            out DateTimeOffset? absoluteExpiration,
            out TimeSpan? slidingExpiration)
        {
            absoluteExpiration = null;
            slidingExpiration = null;
            var absoluteExpirationTicks = (long?) results[0];
            if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != NotPresent)
            {
                absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks.Value, TimeSpan.Zero);
            }

            var slidingExpirationTicks = (long?) results[1];
            if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != NotPresent)
            {
                slidingExpiration = new TimeSpan(slidingExpirationTicks.Value);
            }
        }

        protected virtual void Refresh(
            string key,
            DateTimeOffset? absExpr,
            TimeSpan? sldExpr)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Note Refresh has no effect if there is just an absolute expiration (or neither).
            TimeSpan? expr = null;
            if (sldExpr.HasValue)
            {
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }

                Cache.KeyExpire(Instance + key, expr);
                // TODO: Error handling
            }
        }

        protected virtual async Task RefreshAsync(
            string key,
            DateTimeOffset? absExpr,
            TimeSpan? sldExpr,
            CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            // Note Refresh has no effect if there is just an absolute expiration (or neither).
            TimeSpan? expr = null;
            if (sldExpr.HasValue)
            {
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }

                await Cache.KeyExpireAsync(Instance + key, expr);
                // TODO: Error handling
            }
        }

        protected static long? GetExpirationInSeconds(
            DateTimeOffset creationTime,
            DateTimeOffset? absoluteExpiration,
            DistributedCacheEntryOptions options)
        {
            if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue)
            {
                return (long) Math.Min(
                    (absoluteExpiration.Value - creationTime).TotalSeconds,
                    options.SlidingExpiration.Value.TotalSeconds);
            }
            else if (absoluteExpiration.HasValue)
            {
                return (long) (absoluteExpiration.Value - creationTime).TotalSeconds;
            }
            else if (options.SlidingExpiration.HasValue)
            {
                return (long) options.SlidingExpiration.Value.TotalSeconds;
            }

            return null;
        }

        protected static DateTimeOffset? GetAbsoluteExpiration(
            DateTimeOffset creationTime,
            DistributedCacheEntryOptions options)
        {
            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
                    options.AbsoluteExpiration.Value,
                    "The absolute expiration value must be in the future.");
            }

            var absoluteExpiration = options.AbsoluteExpiration;
            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                absoluteExpiration = creationTime + options.AbsoluteExpirationRelativeToNow;
            }

            return absoluteExpiration;
        }

        public virtual void Dispose()
        {
            Connection?.Close();
        }
    }
}
