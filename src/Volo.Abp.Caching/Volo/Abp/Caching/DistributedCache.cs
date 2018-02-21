using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Threading;

namespace Volo.Abp.Caching
{
    public class DistributedCache<TCacheItem> : IDistributedCache<TCacheItem>
        where TCacheItem : class
    {
        protected IDistributedCache Cache { get; }

        public ICancellationTokenProvider CancellationTokenProvider { get; }

        public DistributedCache(
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task<TCacheItem> GetAsync(string key, CancellationToken token = default)
        {
            var cachedValue = await Cache.GetAsync(key, CancellationTokenProvider.FallbackToProvider(token));
            if (cachedValue == null)
            {
                return null;
            }

            throw new NotImplementedException();
        }

        public Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null,
            CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return Cache.RemoveAsync(key, CancellationTokenProvider.FallbackToProvider(token));
        }
    }
}