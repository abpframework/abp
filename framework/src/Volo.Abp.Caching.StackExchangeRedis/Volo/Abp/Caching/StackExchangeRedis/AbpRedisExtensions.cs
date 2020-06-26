using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Volo.Abp.Caching.StackExchangeRedis
{
    public static class AbpRedisExtensions
    {
        private const string HmGetManyScript = (@"
                                                local res = {};
                                                for i , key in ipairs(KEYS) do
                                                    res[#res+1] = redis.call('HMGET', key,unpack(ARGV))
                                                end
                                                return res");

        public static RedisValue[][] HashMemberGetMany(
            this IDatabase cache,
            string[] keys,
            params string[] members)
        {
            var results = cache.ScriptEvaluate(
                HmGetManyScript,
                keys.Select(key => (RedisKey) key).ToArray(),
                members.Select(member => (RedisValue) member).ToArray());

            return ((RedisResult[]) results).Select(result => (RedisValue[]) result).ToArray();
        }

        public static async Task<RedisValue[][]> HashMemberGetManyAsync(
            this IDatabase cache,
            string[] keys,
            params string[] members)
        {
            var results = await cache.ScriptEvaluateAsync(
                HmGetManyScript,
                keys.Select(key => (RedisKey) key).ToArray(),
                members.Select(member => (RedisValue) member).ToArray());

            return ((RedisResult[]) results).Select(result => (RedisValue[]) result).ToArray();
        }
    }
}
