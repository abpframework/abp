using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;

namespace Volo.Abp.IdentityServer
{
    public class IdentityServerDistributedCache<T> : ICache<T>
        where T : class
    {
        protected ConcurrentBag<string> AllKeys { get; }

        protected IDistributedCache<T> Cache { get; }

        public IdentityServerDistributedCache(IDistributedCache<T> cache)
        {
            Cache = cache;

            AllKeys = new ConcurrentBag<string>();
        }

        public virtual async Task<T> GetAsync(string key)
        {
            var newKey = GetKey(key);
            return await Cache.GetAsync(newKey);
        }

        public virtual async Task SetAsync(string key, T item, TimeSpan expiration)
        {
            var newKey = GetKey(key);

            AllKeys.Add(newKey);

            await Cache.SetAsync(newKey, item, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public virtual async Task RemoveAsync(string key)
        {
            var newKey = GetKey(key);
            await Cache.RemoveAsync(newKey);
        }

        public virtual async Task RemoveAllAsync()
        {
            foreach (var key in AllKeys.Distinct())
            {
                var newKey = GetKey(key);
                await Cache.RemoveAsync(newKey);
            }
        }

        protected virtual string GetKey(string key)
        {
            return key;
        }
    }
}
