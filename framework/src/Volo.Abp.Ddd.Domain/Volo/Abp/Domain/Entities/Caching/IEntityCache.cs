using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace Volo.Abp.Domain.Entities.Caching
{
    public interface IEntityCache<TCacheItem, TKey>
        where TCacheItem : class
    {
        IDistributedCache<TCacheItem> InternalCache { get; }

        TCacheItem this[TKey id] { get; }

        string CacheName { get; }

        TCacheItem Get(TKey id);

        Task<TCacheItem> GetAsync(TKey id);
    }
}
