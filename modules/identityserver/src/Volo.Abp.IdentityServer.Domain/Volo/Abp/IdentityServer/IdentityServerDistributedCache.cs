using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.Reflection;

namespace Volo.Abp.IdentityServer
{
    public class IdentityServerDistributedCache<T> : ICache<T>
        where T : class
    {
        protected ConcurrentDictionary<string, string> AllKeys { get; }

        protected IDistributedCache<IdentityServerDistributedCacheItem<T>> Cache { get; }

        public IdentityServerDistributedCache(IDistributedCache<IdentityServerDistributedCacheItem<T>> cache)
        {
            Cache = cache;

            AllKeys = new ConcurrentDictionary<string, string>();
        }

        public virtual async Task<T> GetAsync(string key)
        {
            return (await Cache.GetAsync(GetCacheKey(key)))?.Value;
        }

        public virtual async Task SetAsync(string key, T item, TimeSpan expiration)
        {
            var newKey = GetCacheKey(key);
            AllKeys.TryAdd(newKey, newKey);
            await Cache.SetAsync(newKey, new IdentityServerDistributedCacheItem<T>(item), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public virtual async Task RemoveAsync(string key)
        {
            var newKey = GetCacheKey(key);
            AllKeys.TryRemove(newKey, out _);
            await Cache.RemoveAsync(newKey);
        }

        public virtual async Task RemoveAllAsync()
        {
            var keys = AllKeys.Keys.ToArray();
            AllKeys.Clear();
            foreach (var key in keys.Distinct())
            {
                await Cache.RemoveAsync(key);
            }
        }

        protected virtual string GetCacheKey(string key)
        {
            return TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(T)) + ":" + key;
        }
    }
}
