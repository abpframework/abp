using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;

namespace Volo.Abp.Caching
{
    public interface IDistributedCache<TCacheItem>
        where TCacheItem : class
    {
        Task<TCacheItem> GetAsync(
            [NotNull] string key,
            CancellationToken token = default
        );

        Task SetAsync(
            [NotNull] string key,
            [NotNull] TCacheItem value,
            [CanBeNull] DistributedCacheEntryOptions options = null,
            CancellationToken token = default
        );

        Task RemoveAsync(
            string key,
            CancellationToken token = default
        );
    }
}
