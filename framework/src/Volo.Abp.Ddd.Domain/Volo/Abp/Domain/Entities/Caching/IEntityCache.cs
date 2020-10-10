using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace Volo.Abp.Domain.Entities.Caching
{
    public interface IEntityCache<TCacheItem, TKey>
        where TCacheItem : class
    {
        IDistributedCache<TCacheItem> InternalCache { get; }

        TCacheItem this[TKey key] { get; }

        string CacheName { get; }

        TCacheItem Get(TKey key);

        Task<TCacheItem> GetAsync(TKey key);
    }
}
