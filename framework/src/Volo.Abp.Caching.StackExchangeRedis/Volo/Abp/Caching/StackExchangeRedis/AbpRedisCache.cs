using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Caching.StackExchangeRedis;

[DisableConventionalRegistration]
public class AbpRedisCache : RedisCache, ICacheSupportsMultipleItems
{
    protected readonly static string AbsoluteExpirationKey;
    protected readonly static string SlidingExpirationKey;
    protected readonly static string DataKey;
    protected readonly static long NotPresent;
    protected readonly static RedisValue[] HashMembersAbsoluteExpirationSlidingExpirationData;
    protected readonly static RedisValue[] HashMembersAbsoluteExpirationSlidingExpiration;

    protected readonly static FieldInfo RedisDatabaseField;
    protected readonly static MethodInfo ConnectMethod;
    protected readonly static MethodInfo ConnectAsyncMethod;
    protected readonly static MethodInfo MapMetadataMethod;
    protected readonly static MethodInfo GetAbsoluteExpirationMethod;
    protected readonly static MethodInfo GetExpirationInSecondsMethod;
    protected readonly static MethodInfo OnRedisErrorMethod;
    protected readonly static MethodInfo RecycleMethodInfo;

    protected RedisKey InstancePrefix { get; }

    static AbpRedisCache()
    {
        var type = typeof(RedisCache);

        RedisDatabaseField = Check.NotNull(type.GetField("_cache", BindingFlags.Instance | BindingFlags.NonPublic), nameof(RedisDatabaseField));

        ConnectMethod = Check.NotNull(type.GetMethod("Connect", BindingFlags.Instance | BindingFlags.NonPublic), nameof(ConnectMethod));

        ConnectAsyncMethod = Check.NotNull(type.GetMethod("ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic), nameof(ConnectAsyncMethod));

        MapMetadataMethod = Check.NotNull(type.GetMethod("MapMetadata", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static), nameof(MapMetadataMethod));

        GetAbsoluteExpirationMethod = Check.NotNull(type.GetMethod("GetAbsoluteExpiration", BindingFlags.Static | BindingFlags.NonPublic), nameof(GetAbsoluteExpirationMethod));

        GetExpirationInSecondsMethod = Check.NotNull(type.GetMethod("GetExpirationInSeconds", BindingFlags.Static | BindingFlags.NonPublic), nameof(GetExpirationInSecondsMethod));

        OnRedisErrorMethod = Check.NotNull(type.GetMethod("OnRedisError", BindingFlags.Instance | BindingFlags.NonPublic), nameof(OnRedisErrorMethod));

        RecycleMethodInfo = Check.NotNull(type.GetMethod("Recycle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static), nameof(RecycleMethodInfo));

        AbsoluteExpirationKey = type.GetField("AbsoluteExpirationKey", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!.ToString()!;

        SlidingExpirationKey = type.GetField("SlidingExpirationKey", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!.ToString()!;

        DataKey = type.GetField("DataKey", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!.ToString()!;

        NotPresent = type.GetField("NotPresent", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!.To<int>();

        HashMembersAbsoluteExpirationSlidingExpirationData = [AbsoluteExpirationKey, SlidingExpirationKey, DataKey];

        HashMembersAbsoluteExpirationSlidingExpiration = [AbsoluteExpirationKey, SlidingExpirationKey];
    }

    public AbpRedisCache(IOptions<RedisCacheOptions> optionsAccessor)
        : base(optionsAccessor)
    {
        var instanceName = optionsAccessor.Value.InstanceName;
        if (!string.IsNullOrEmpty(instanceName))
        {
            InstancePrefix = (RedisKey)Encoding.UTF8.GetBytes(instanceName);
        }
    }

    protected virtual IDatabase Connect()
    {
        return (IDatabase)ConnectMethod.Invoke(this, Array.Empty<object>())!;
    }

    protected virtual async ValueTask<IDatabase> ConnectAsync(CancellationToken token = default)
    {
        return await (ValueTask<IDatabase>)ConnectAsyncMethod.Invoke(this, new object[] { token })!;
    }

    protected virtual void Recycle(byte[]? lease)
    {
        RecycleMethodInfo.Invoke(this, new object[] { lease! });
    }

    public virtual byte[]?[] GetMany(IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        return GetAndRefreshMany(keys, true);
    }

    public virtual async Task<byte[]?[]> GetManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        return await GetAndRefreshManyAsync(keys, true, token);
    }

    public virtual void SetMany(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options)
    {
        var cache = Connect();

        try
        {
            Task.WaitAll(PipelineSetMany(cache, items, options, out var leases));
            foreach (var lease in leases)
            {
                Recycle(lease);
            }
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    public virtual async Task SetManyAsync(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var cache = await ConnectAsync(token);

        try
        {
            await Task.WhenAll(PipelineSetMany(cache, items, options, out var leases));
            foreach (var lease in leases)
            {
                Recycle(lease);
            }
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    public virtual void RefreshMany(IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        GetAndRefreshMany(keys, false);
    }

    public virtual async Task RefreshManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        await GetAndRefreshManyAsync(keys, false, token);
    }

    public virtual void RemoveMany(IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        var cache = Connect();

        try
        {
            Task.WaitAll(PipelineRemoveManyAsync(cache, keys));
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    public virtual async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        token.ThrowIfCancellationRequested();
        var cache = await ConnectAsync(token);

        try
        {
            await Task.WhenAll(PipelineRemoveManyAsync(cache, keys));
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }
    
    protected virtual Task[] PipelineRemoveManyAsync(IDatabase cache, IEnumerable<string> keys)
    {
        return keys.Select(key => cache.KeyDeleteAsync(InstancePrefix.Append(key))).ToArray<Task>();
    }

    protected virtual byte[]?[] GetAndRefreshMany(IEnumerable<string> keys, bool getData)
    {
       var cache = Connect();

        var keyArray = keys.Select(key => InstancePrefix.Append( key)).ToArray();
        byte[]?[] bytes;

        try
        {
            var results = cache.HashMemberGetMany(keyArray, GetHashFields(getData));

            Task.WaitAll(PipelineRefreshManyAndOutData(cache, keyArray, results, out bytes));
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }

        return bytes;
    }

    protected virtual async Task<byte[]?[]> GetAndRefreshManyAsync(IEnumerable<string> keys, bool getData, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var cache = await ConnectAsync(token);

        var keyArray = keys.Select(key => InstancePrefix.Append(key)).ToArray();
        byte[]?[] bytes;

        try
        {
            var results = await cache.HashMemberGetManyAsync(keyArray, GetHashFields(getData));
            await Task.WhenAll(PipelineRefreshManyAndOutData(cache, keyArray, results, out bytes));
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }

        return bytes;
    }

    protected virtual Task[] PipelineRefreshManyAndOutData(IDatabase cache, RedisKey[] keys, RedisValue[][] results, out byte[]?[] bytes)
    {
        bytes = new byte[keys.Length][];
        var tasks = new Task[keys.Length];

        for (var i = 0; i < keys.Length; i++)
        {
            if (results[i].Length >= 2)
            {
                MapMetadata(results[i], out var absExpr, out var sldExpr);

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

                    tasks[i] = cache.KeyExpireAsync(keys[i], expr);
                }
                else
                {
                    tasks[i] = Task.CompletedTask;
                }
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

    protected virtual Task[] PipelineSetMany(IDatabase cache, IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options, out List<byte[]?> leases)
    {
        var tasks = new List<Task>();
        leases = new List<byte[]?>();

        var creationTime = DateTimeOffset.UtcNow;

        var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

        foreach (var item in items)
        {
            var prefixedKey = InstancePrefix.Append(item.Key);
            var ttl = GetExpirationInSeconds(creationTime, absoluteExpiration, options);
            var fields = GetHashFields(Linearize(new ReadOnlySequence<byte>(item.Value), out var lease), absoluteExpiration, options.SlidingExpiration);
            leases.Add(lease);
            if (ttl is null)
            {
                tasks.Add(cache.HashSetAsync(prefixedKey, fields));
            }
            else
            {
                tasks.Add(cache.HashSetAsync(prefixedKey, fields));
                tasks.Add(cache.KeyExpireAsync(prefixedKey, TimeSpan.FromSeconds(ttl.GetValueOrDefault())));
            }
        }

        return tasks.ToArray();
    }

    protected virtual void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
    {
        var parameters = new object?[] { results, null, null };
        MapMetadataMethod.Invoke(this, parameters);

        absoluteExpiration = (DateTimeOffset?)parameters[1];
        slidingExpiration = (TimeSpan?)parameters[2];
    }

    protected virtual long? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
    {
        return (long?)GetExpirationInSecondsMethod.Invoke(null, new object?[] { creationTime, absoluteExpiration, options });
    }

    protected virtual DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options)
    {
        return (DateTimeOffset?)GetAbsoluteExpirationMethod.Invoke(null, new object[] { creationTime, options });
    }

    protected virtual void OnRedisError(Exception ex, IDatabase cache)
    {
        OnRedisErrorMethod.Invoke(this, [ex, cache]);
    }

    private static ReadOnlyMemory<byte> Linearize(in ReadOnlySequence<byte> value, out byte[]? lease)
    {
        // RedisValue only supports single-segment chunks; this will almost never be an issue, but
        // on those rare occasions: use a leased array to harmonize things
        if (value.IsSingleSegment)
        {
            lease = null;
            return value.First;
        }
        var length = checked((int)value.Length);
        lease = ArrayPool<byte>.Shared.Rent(length);
        value.CopyTo(lease);
        return new(lease, 0, length);
    }

    private static RedisValue[] GetHashFields(bool getData)
    {
        return getData
            ? HashMembersAbsoluteExpirationSlidingExpirationData
            : HashMembersAbsoluteExpirationSlidingExpiration;
    }

    private static HashEntry[] GetHashFields(RedisValue value, DateTimeOffset? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        return
        [
            new HashEntry(AbsoluteExpirationKey, absoluteExpiration?.Ticks ?? NotPresent),
            new HashEntry(SlidingExpirationKey, slidingExpiration?.Ticks ?? NotPresent),
            new HashEntry(DataKey, value)
        ];
    }
}
