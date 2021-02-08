using System;
using System.Threading.Tasks;
using IdentityServer4.Services;

namespace Volo.Abp.IdentityServer
{
    public class IdentityServerDistributedCacheWrapper<T> : ICache<T>
        where T : class
    {
        private readonly IdentityServerDistributedCache<T> _cache;

        public IdentityServerDistributedCacheWrapper(IdentityServerDistributedCache<T> cache)
        {
            _cache = cache;
        }

        public virtual async Task<T> GetAsync(string key)
        {
            return await _cache.GetAsync(key);
        }

        public virtual async Task SetAsync(string key, T item, TimeSpan expiration)
        {
            await _cache.SetAsync(key, item, expiration);
        }
    }
}
