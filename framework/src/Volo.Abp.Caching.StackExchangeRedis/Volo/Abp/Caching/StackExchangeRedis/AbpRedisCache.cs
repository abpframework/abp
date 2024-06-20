using System;
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
    protected static readonly string AbsoluteExpirationKey;
    protected static readonly string SlidingExpirationKey;
    protected static readonly string DataKey;
    protected static readonly long NotPresent;
    protected static readonly RedisValue[] HashMembersAbsoluteExpirationSlidingExpirationData;
    protected static readonly RedisValue[] HashMembersAbsoluteExpirationSlidingExpiration;

    private readonly static FieldInfo SetScriptField;
    private readonly static FieldInfo RedisDatabaseField;
    private readonly static MethodInfo ConnectMethod;
    private readonly static MethodInfo ConnectAsyncMethod;
    private readonly static MethodInfo MapMetadataMethod;
    private readonly static MethodInfo GetAbsoluteExpirationMethod;
    private readonly static MethodInfo GetExpirationInSecondsMethod;
    private readonly static MethodInfo OnRedisErrorMethod;

    protected RedisKey InstancePrefix { get; }

    static AbpRedisCache()
    {
        var type = typeof(RedisCache);

        RedisDatabaseField = Check.NotNull(type.GetField("_cache", BindingFlags.Instance | BindingFlags.NonPublic), nameof(RedisDatabaseField));

        SetScriptField = Check.NotNull(type.GetField("_setScript", BindingFlags.Instance | BindingFlags.NonPublic), nameof(SetScriptField));
        
        ConnectMethod = Check.NotNull(type.GetMethod("Connect", BindingFlags.Instance | BindingFlags.NonPublic), nameof(ConnectMethod));

        ConnectAsyncMethod = Check.NotNull(type.GetMethod("ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic), nameof(ConnectAsyncMethod));

        MapMetadataMethod = Check.NotNull(type.GetMethod("MapMetadata", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static), nameof(MapMetadataMethod));

        GetAbsoluteExpirationMethod = Check.NotNull(type.GetMethod("GetAbsoluteExpiration", BindingFlags.Static | BindingFlags.NonPublic), nameof(GetAbsoluteExpirationMethod));

        GetExpirationInSecondsMethod = Check.NotNull(type.GetMethod("GetExpirationInSeconds", BindingFlags.Static | BindingFlags.NonPublic), nameof(GetExpirationInSecondsMethod));
        
        OnRedisErrorMethod = Check.NotNull(type.GetMethod("OnRedisError", BindingFlags.Instance | BindingFlags.NonPublic), nameof(OnRedisErrorMethod));

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

    public byte[]?[] GetMany(
        IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        return GetAndRefreshMany(keys, true);
    }

    public async Task<byte[]?[]> GetManyAsync(
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
        var cache = Connect();

        try
        {
            Task.WaitAll(PipelineSetMany(cache, items, options));
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    public async Task SetManyAsync(
        IEnumerable<KeyValuePair<string, byte[]>> items,
        DistributedCacheEntryOptions options,
        CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var cache = await ConnectAsync(token);

        try
        {
            await Task.WhenAll(PipelineSetMany(cache, items, options));
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    public void RefreshMany(
        IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        GetAndRefreshMany(keys, false);
    }

    public async Task RefreshManyAsync(
        IEnumerable<string> keys,
        CancellationToken token = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        await GetAndRefreshManyAsync(keys, false, token);
    }

    public void RemoveMany(IEnumerable<string> keys)
    {
        keys = Check.NotNull(keys, nameof(keys));

        var cache = Connect();

        try
        {
            cache.KeyDelete(keys.Select(key => InstancePrefix.Append(key)).ToArray());
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        keys = Check.NotNull(keys, nameof(keys));

        token.ThrowIfCancellationRequested();
        var cache = await ConnectAsync(token);

        try
        {
            await cache.KeyDeleteAsync(keys.Select(key => InstancePrefix.Append(key)).ToArray());
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    protected virtual byte[]?[] GetAndRefreshMany(
        IEnumerable<string> keys,
        bool getData)
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

    protected virtual async Task<byte[]?[]> GetAndRefreshManyAsync(
        IEnumerable<string> keys,
        bool getData,
        CancellationToken token = default)
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

    protected virtual Task[] PipelineRefreshManyAndOutData(
        IDatabase cache,
        RedisKey[] keys,
        RedisValue[][] results,
        out byte[]?[] bytes)
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

    protected virtual Task[] PipelineSetMany(
        IDatabase cache,
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
            tasks[i] = cache.ScriptEvaluateAsync(GetSetScript(), new RedisKey[] { InstancePrefix.Append(itemArray[i].Key) },
            [
                absoluteExpiration?.Ticks ?? NotPresent,
                        options.SlidingExpiration?.Ticks ?? NotPresent,
                        GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                        itemArray[i].Value
            ]);
        }

        return tasks;
    }

    protected virtual void MapMetadata(
        RedisValue[] results,
        out DateTimeOffset? absoluteExpiration,
        out TimeSpan? slidingExpiration)
    {
        var parameters = new object?[] { results, null, null };
        MapMetadataMethod.Invoke(this, parameters);

        absoluteExpiration = (DateTimeOffset?)parameters[1];
        slidingExpiration = (TimeSpan?)parameters[2];
    }

    protected virtual long? GetExpirationInSeconds(
        DateTimeOffset creationTime,
        DateTimeOffset? absoluteExpiration,
        DistributedCacheEntryOptions options)
    {
        return (long?)GetExpirationInSecondsMethod.Invoke(null,
            new object?[] { creationTime, absoluteExpiration, options });
    }

    protected virtual DateTimeOffset? GetAbsoluteExpiration(
        DateTimeOffset creationTime,
        DistributedCacheEntryOptions options)
    {
        return (DateTimeOffset?)GetAbsoluteExpirationMethod.Invoke(null, new object[] { creationTime, options });
    }
    
    protected virtual void OnRedisError(Exception ex, IDatabase cache)
    {
        OnRedisErrorMethod.Invoke(this, [ex, cache]);
    }
    
    private string GetSetScript()
    {
        return SetScriptField.GetValue(this)!.ToString()!;
    }
    
    private static RedisValue[] GetHashFields(bool getData)
    {
        return getData
            ? HashMembersAbsoluteExpirationSlidingExpirationData
            : HashMembersAbsoluteExpirationSlidingExpiration;
    }
}
