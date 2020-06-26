using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Caching.StackExchangeRedis
{
    [DisableConventionalRegistration]
    public class AbpRedisCache : RedisCache, ICacheSupportsMultipleItems
    {
        public AbpRedisCache(IOptions<RedisCacheOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }

        public byte[][] GetMany(
            IEnumerable<string> keys)
        {
            keys = Check.NotNull(keys, nameof(keys));

            return GetAndRefreshMany(keys, true);
        }

        public async Task<byte[][]> GetManyAsync(
            IEnumerable<string> keys,
            CancellationToken token = default)
        {
            keys = Check.NotNull(keys, nameof(keys));

            return await GetAndRefreshManyAsync(keys, true, token);
        }

        public void SetMany(
            IEnumerable<KeyValuePair<string, byte[]>> items,
            DistributedCacheEntryOptions options)
        {
            Connect();

            Task.WaitAll(PipelineSetMany(items, options));
        }

        public async Task SetManyAsync(
            IEnumerable<KeyValuePair<string, byte[]>> items,
            DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            await Task.WhenAll(PipelineSetMany(items, options));
        }

        protected virtual byte[][] GetAndRefreshMany(
            IEnumerable<string> keys,
            bool getData)
        {
            Connect();

            var keyArray = keys.Select(key => Instance + key).ToArray();
            RedisValue[][] results;

            if (getData)
            {
                results = Cache.HashMemberGetMany(keyArray, AbsoluteExpirationKey,
                    SlidingExpirationKey, DataKey);
            }
            else
            {
                results = Cache.HashMemberGetMany(keyArray, AbsoluteExpirationKey,
                    SlidingExpirationKey);
            }

            Task.WaitAll(GetAndRefreshMany(keyArray, results, out var bytes));

            return bytes;
        }

        protected virtual async Task<byte[][]> GetAndRefreshManyAsync(
            IEnumerable<string> keys,
            bool getData,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            var keyArray = keys.Select(key => Instance + key).ToArray();
            RedisValue[][] results;

            if (getData)
            {
                results = await Cache.HashMemberGetManyAsync(keyArray, AbsoluteExpirationKey,
                    SlidingExpirationKey, DataKey);
            }
            else
            {
                results = await Cache.HashMemberGetManyAsync(keyArray, AbsoluteExpirationKey,
                    SlidingExpirationKey);
            }

            await Task.WhenAll(GetAndRefreshMany(keyArray, results, out var bytes));

            return bytes;
        }

        private Task[] GetAndRefreshMany(string[] keys, RedisValue[][] results, out byte[][] bytes)
        {
            bytes = new byte[keys.Length][];
            var tasks = new Task[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                if (results[i].Length >= 2)
                {
                    MapMetadata(results[i], out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                    tasks[i] = PipelineRefresh(keys[i], absExpr, sldExpr);
                }

                if (results[i].Length >= 3 && results[i][2].HasValue)
                {
                    bytes[i] = results[i][2];
                }
                else
                {
                    bytes[i] = null;
                }
            }

            return tasks;
        }

        private Task PipelineRefresh(
            string key,
            DateTimeOffset? absExpr,
            TimeSpan? sldExpr)
        {
            if (sldExpr.HasValue)
            {
                TimeSpan? expr;

                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }

                return Cache.KeyExpireAsync(key, expr);
            }

            return Task.CompletedTask;
        }

        private Task[] PipelineSetMany(
            IEnumerable<KeyValuePair<string, byte[]>> items,
            DistributedCacheEntryOptions options)
        {
            items = Check.NotNull(items, nameof(items));
            options = Check.NotNull(options, nameof(options));

            var itemArray = items.ToArray();
            var tasks = new Task[itemArray.Length];
            var creationTime = DateTimeOffset.UtcNow;
            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

            for (var i = 0; i < itemArray.Length; i++)
            {
                tasks[i] = Cache.ScriptEvaluateAsync(SetScript, new RedisKey[] {Instance + itemArray[i].Key},
                    new RedisValue[]
                    {
                        absoluteExpiration?.Ticks ?? NotPresent,
                        options.SlidingExpiration?.Ticks ?? NotPresent,
                        GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                        itemArray[i].Value
                    });
            }

            return tasks;
        }
    }
}
