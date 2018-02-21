using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Serialization;
using Volo.Abp.Threading;

namespace Volo.Abp.Caching
{
    public class DistributedCache<TCacheItem> : IDistributedCache<TCacheItem>
        where TCacheItem : class
    {
        protected IDistributedCache Cache { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }
        protected IObjectSerializer ObjectSerializer { get; }

        public DistributedCache(
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IObjectSerializer objectSerializer)
        {
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
            ObjectSerializer = objectSerializer;
        }

        public TCacheItem Get(string key)
        {
            var cachedBytes = Cache.Get(key);
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        public async Task<TCacheItem> GetAsync(string key, CancellationToken token = default)
        {
            var cachedBytes = await Cache.GetAsync(key, CancellationTokenProvider.FallbackToProvider(token));
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        public void Set(string key, TCacheItem value, DistributedCacheEntryOptions options)
        {
            Cache.Set(
                key,
                ObjectSerializer.Serialize(value),
                options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) } //TODO: implement per cache item and global defaults!!!
            );
        }

        public Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, CancellationToken token = default)
        {
            return Cache.SetAsync(
                key,
                ObjectSerializer.Serialize(value),
                options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) }, //TODO: implement per cache item and global defaults!!!
                CancellationTokenProvider.FallbackToProvider(token)
            );
        }

        public void Refresh(string key)
        {
            Cache.Refresh(key);
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return Cache.RefreshAsync(key, CancellationTokenProvider.FallbackToProvider(token));
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return Cache.RemoveAsync(key, CancellationTokenProvider.FallbackToProvider(token));
        }
    }
}