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
            string key
        );

        Task<TCacheItem> GetAsync(
            [NotNull] string key,
            CancellationToken token = default
        );

        void Set(
            string key,
            TCacheItem value,
            DistributedCacheEntryOptions options
        );

        Task SetAsync(
            [NotNull] string key,
            [NotNull] TCacheItem value,
            [CanBeNull] DistributedCacheEntryOptions options = null,
            CancellationToken token = default
        );

        void Refresh(
            string key
        );

        Task RefreshAsync(
            string key,
            CancellationToken token = default
        );

        void Remove(
            string key
        );

        Task RemoveAsync(
            string key,
            CancellationToken token = default
        );
    }
}
