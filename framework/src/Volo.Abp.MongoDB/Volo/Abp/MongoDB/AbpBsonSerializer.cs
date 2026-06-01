using System;
using System.Collections.Concurrent;

using System.Reflection;
using MongoDB.Bson.Serialization;

namespace Volo.Abp.MongoDB;

public static class AbpBsonSerializer
{
    private static readonly ConcurrentDictionary<Type, IBsonSerializer> Cache;

    static AbpBsonSerializer()
    {
        var registry = BsonSerializer.SerializerRegistry;
        var type = typeof(BsonSerializerRegistry);
        var cacheField = type.GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance) ??
                         throw new AbpException($"Cannot find _cache field of {type.FullName}.");
        Cache = (ConcurrentDictionary<Type, IBsonSerializer>)cacheField.GetValue(registry)!;
    }

    public static void RemoveSerializer<T>()
    {
        Cache.TryRemove(typeof(T), out _);
    }

    public static ConcurrentDictionary<Type, IBsonSerializer> GetSerializerCache()
    {
        return Cache;
    }
}
