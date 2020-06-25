using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Caching.StackExchangeRedis
{
    [DisableConventionalRegistration]
    public class AbpRedisCache : RedisCache
    {
        private static readonly FieldInfo RedisDatabaseField;
        private static readonly MethodInfo ConnectMethod;
        private static readonly MethodInfo ConnectAsyncMethod;

        protected IDatabase RedisDatabase => RedisDatabaseField.GetValue(this) as IDatabase;

        static AbpRedisCache()
        {
            RedisDatabaseField = typeof(RedisCache)
                .GetField("_cache", BindingFlags.Instance | BindingFlags.NonPublic);
            
            ConnectMethod = typeof(RedisCache)
                .GetMethod("Connect", BindingFlags.Instance | BindingFlags.NonPublic);
            
            ConnectAsyncMethod = typeof(RedisCache)
                .GetMethod("ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public AbpRedisCache(IOptions<RedisCacheOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            
        }

        protected virtual void Connect()
        {
            ConnectMethod.Invoke(this, Array.Empty<object>());
        }

        protected virtual Task ConnectAsync(CancellationToken token = default)
        {
            return (Task) ConnectAsyncMethod.Invoke(this, new object[] {token});
        }
    }
}