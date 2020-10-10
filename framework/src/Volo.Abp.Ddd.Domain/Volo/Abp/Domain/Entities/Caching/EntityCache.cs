using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Domain.Entities.Caching
{
    public class EntityCache<TEntity, TCacheItem, TKey> :
        AbstractKeyEntityCache<TEntity, TCacheItem, TKey>
        where TCacheItem : class
        where TEntity : class, IEntity<TKey>
    {
        protected new IRepository<TEntity, TKey> Repository { get; }

        public EntityCache(
            IRepository<TEntity, TKey> repository,
            IDistributedCache<TCacheItem> internalCache,
            string cacheName = null) : base(repository, internalCache, cacheName)
        {
            Repository = repository;
        }

        protected override TEntity GetEntityFromDataSource(TKey key)
        {
            return Repository.FirstOrDefault(entity => entity.Id.Equals(key));
        }

        protected override async Task<TEntity> GetEntityFromDataSourceAsync(TKey key)
        {
            return await Repository.FirstOrDefaultAsync(entity => entity.Id.Equals(key));
        }

        protected override string GetCacheKey(TKey key)
        {
            return key.ToString();
        }

        protected override string GetCacheKey(TEntity entity)
        {
            return GetCacheKey(entity.Id);
        }
    }
}
