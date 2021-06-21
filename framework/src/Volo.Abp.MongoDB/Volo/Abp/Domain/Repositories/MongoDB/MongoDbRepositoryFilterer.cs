using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepositoryFilterer<TEntity> : IMongoDbRepositoryFilterer<TEntity>
        where TEntity : class, IEntity
    {
        protected IDataFilter DataFilter { get; }

        protected ICurrentTenant CurrentTenant { get; }

        public MongoDbRepositoryFilterer(IDataFilter dataFilter, ICurrentTenant currentTenant)
        {
            DataFilter = dataFilter;
            CurrentTenant = currentTenant;
        }

        public virtual void AddGlobalFilters(List<FilterDefinition<TEntity>> filters)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && DataFilter.IsEnabled<ISoftDelete>())
            {
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((ISoftDelete)e).IsDeleted, false));
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                var tenantId = CurrentTenant.Id;
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((IMultiTenant)e).TenantId, tenantId));
            }
        }
    }

    public class MongoDbRepositoryFilterer<TEntity, TKey> : MongoDbRepositoryFilterer<TEntity>,
        IMongoDbRepositoryFilterer<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public MongoDbRepositoryFilterer(IDataFilter dataFilter, ICurrentTenant currentTenant)
            : base(dataFilter, currentTenant)
        {
        }

        public FilterDefinition<TEntity> CreateEntityFilter(TKey id, bool applyFilters = false)
        {
            var filters = new List<FilterDefinition<TEntity>>
            {
                Builders<TEntity>.Filter.Eq(e => e.Id, id)
            };

            if (applyFilters)
            {
                AddGlobalFilters(filters);
            }

            return Builders<TEntity>.Filter.And(filters);
        }

        public FilterDefinition<TEntity> CreateEntityFilter(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
        {
            if (!withConcurrencyStamp || !(entity is IHasConcurrencyStamp entityWithConcurrencyStamp))
            {
                return Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            }

            if (concurrencyStamp == null)
            {
                concurrencyStamp = entityWithConcurrencyStamp.ConcurrencyStamp;
            }

            return Builders<TEntity>.Filter.And(
                Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                Builders<TEntity>.Filter.Eq(e => ((IHasConcurrencyStamp)e).ConcurrencyStamp, concurrencyStamp)
            );
        }

        public FilterDefinition<TEntity> CreateEntitiesFilter(IEnumerable<TEntity> entities, bool applyFilters = false)
        {
            return CreateEntitiesFilter(entities.Select(s => s.Id), applyFilters);
        }

        public FilterDefinition<TEntity> CreateEntitiesFilter(IEnumerable<TKey> ids, bool applyFilters = false)
        {
            var filters = new List<FilterDefinition<TEntity>>()
            {
                Builders<TEntity>.Filter.In(e => e.Id, ids),
            };

            if (applyFilters)
            {
                AddGlobalFilters(filters);
            }

            return Builders<TEntity>.Filter.And(filters);
        }
    }
}