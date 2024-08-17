using System.Threading.Tasks;
using StackExchange.Redis;

namespace Volo.Abp.Caching.StackExchangeRedis;

public static class AbpRedisExtensions
{
    public static RedisValue[][] HashMemberGetMany(
        this IDatabase cache,
        RedisKey[] keys,
        RedisValue[] fields)
    {
        var tasks = new Task<RedisValue[]>[keys.Length];
        var results = new RedisValue[keys.Length][];

        for (var i = 0; i < keys.Length; i++)
        {
            tasks[i] = cache.HashGetAsync(keys[i], fields);
        }

        for (var i = 0; i < tasks.Length; i++)
        {
            results[i] = cache.Wait(tasks[i]);
        }

        return results;
    }

    public async static Task<RedisValue[][]> HashMemberGetManyAsync(
        this IDatabase cache,
        RedisKey[] keys,
        RedisValue[] fields)
    {
        var tasks = new Task<RedisValue[]>[keys.Length];

        for (var i = 0; i < keys.Length; i++)
        {
            tasks[i] = cache.HashGetAsync(keys[i], fields);
        }

        return await Task.WhenAll(tasks);
    }
}
