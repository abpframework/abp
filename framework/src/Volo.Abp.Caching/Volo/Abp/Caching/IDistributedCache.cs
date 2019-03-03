using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;

namespace Volo.Abp.Caching
{
    public interface IDistributedCache<TCacheItem>
        where TCacheItem : class
    {
        TCacheItem Get(
            string key,
            bool? hideErrors = null
        );

        Task<TCacheItem> GetAsync(
            [NotNull] string key,
            bool? hideErrors = null,
            CancellationToken token = default
        );

        TCacheItem GetOrAdd(
            string key,
            Func<TCacheItem> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null
        );

        Task<TCacheItem> GetOrAddAsync(
            [NotNull] string key,
            Func<Task<TCacheItem>> factory,
            Func<DistributedCacheEntryOptions> optionsFactory = null,
            bool? hideErrors = null,
            CancellationToken token = default
        );

        void Set(
            string key,
            TCacheItem value,
            DistributedCacheEntryOptions options = null,
            bool? hideErrors = null
        );

        Task SetAsync(
            [NotNull] string key,
            [NotNull] TCacheItem value,
            [CanBeNull] DistributedCacheEntryOptions options = null,
            bool? hideErrors = null,
            CancellationToken token = default
        );

        void Refresh(
            string key,
            bool? hideErrors = null
        );

        Task RefreshAsync(
            string key,
            bool? hideErrors = null,
            CancellationToken token = default
        );

        void Remove(
            string key,
            bool? hideErrors = null
        );

        Task RemoveAsync(
            string key,
            bool? hideErrors = null,
            CancellationToken token = default
        );
    }
}
