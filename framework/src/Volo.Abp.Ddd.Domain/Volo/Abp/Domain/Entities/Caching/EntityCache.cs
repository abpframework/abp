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
            IDistributedCache<TCacheItem> internalCache) : base(repository, internalCache)
        {
            Repository = repository;
        }

        protected async override Task<TEntity> GetEntityFromDataSourceAsync(TKey key)
        {
            return await Repository.FirstOrDefaultAsync(entity => entity.Id.Equals(key));
        }
    }
}
