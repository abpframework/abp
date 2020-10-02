using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Domain.Entities.Caching
{
    public class EntityCache<TEntity, TCacheItem, TKey> :
        AbstractKeyEntityCache<TEntity, TCacheItem, TKey>
        where TCacheItem : class
        where TEntity : class, IEntity<TKey>
    {
        public EntityCache(
            IRepository<TEntity> repository,
            IDistributedCache<TCacheItem> internalCache,
            string cacheName = null) : base(repository, internalCache, cacheName)
        {
        }

        protected override TEntity GetEntityFromDataSource(TKey id)
        {
            return Repository.FirstOrDefault(entity => entity.Id.Equals(id));
        }

        protected override async Task<TEntity> GetEntityFromDataSourceAsync(TKey id)
        {
            return await Repository.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        protected override string GetCacheKey(TKey id)
        {
            return id.ToString();
        }

        protected override string GetCacheKey(TEntity entity)
        {
            return GetCacheKey(entity.Id);
        }
    }
}
