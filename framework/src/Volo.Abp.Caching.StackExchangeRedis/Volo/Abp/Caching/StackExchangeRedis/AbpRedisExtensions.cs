using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Volo.Abp.Caching.StackExchangeRedis;

public static class AbpRedisExtensions
{
    public static RedisValue[][] HashMemberGetMany(
        this IDatabase cache,
        string[] keys,
        params string[] members)
    {
        var tasks = new Task<RedisValue[]>[keys.Length];
        var fields = members.Select(member => (RedisValue)member).ToArray();
        var results = new RedisValue[keys.Length][];

        for (var i = 0; i < keys.Length; i++)
        {
            tasks[i] = cache.HashGetAsync((RedisKey)keys[i], fields);
        }

        for (var i = 0; i < tasks.Length; i++)
        {
            results[i] = cache.Wait(tasks[i]);
        }

        return results;
    }

    public static async Task<RedisValue[][]> HashMemberGetManyAsync(
        this IDatabase cache,
        string[] keys,
        params string[] members)
    {
        var tasks = new Task<RedisValue[]>[keys.Length];
        var fields = members.Select(member => (RedisValue)member).ToArray();

        for (var i = 0; i < keys.Length; i++)
        {
            tasks[i] = cache.HashGetAsync((RedisKey)keys[i], fields);
        }

        return await Task.WhenAll(tasks);
    }
}
