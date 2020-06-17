using System;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkCache<TCacheItem>
        where TCacheItem : class
    {
        Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory);

        Task<TCacheItem> GetOrNullAsync(string key);

        Task<TCacheItem> SetAsync(string key, TCacheItem item);

        Task RemoveAsync(string key);
    }

    public interface IUnitOfWorkCacheWithFallback<TCacheItem> : IUnitOfWorkCache<TCacheItem>
        where TCacheItem : class
    {
        Task<TCacheItem> GetFallbackCacheItem(string key);

        Task<TCacheItem> SetFallbackCacheItem(string key, TCacheItem item);
    }
}
