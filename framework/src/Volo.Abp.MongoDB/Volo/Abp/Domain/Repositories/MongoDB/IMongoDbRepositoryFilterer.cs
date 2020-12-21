using System.Collections.Generic;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public interface IMongoDbRepositoryFilterer<TEntity> where TEntity : class, IEntity
    {
        void AddGlobalFilters(List<FilterDefinition<TEntity>> filters);
    }

    public interface IMongoDbRepositoryFilterer<TEntity, TKey> : IMongoDbRepositoryFilterer<TEntity> where TEntity : class, IEntity<TKey>
    {
        FilterDefinition<TEntity> CreateEntityFilter(TKey id, bool applyFilters = false);

        FilterDefinition<TEntity> CreateEntityFilter(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null);

        /// <summary>
        /// Creates filter for given entities.
        /// </summary>
        /// <remarks>
        /// Visit https://docs.mongodb.com/manual/reference/operator/query/in/ to get more information about 'in' operator.
        /// </remarks>
        /// <param name="entities">Entities to be filtered.</param>
        /// <param name="applyFilters">Set true to use GlobalFilters. Default is false.</param>
        /// <returns>Created <see cref="FilterDefinition{TDocument}"/>.</returns>
        FilterDefinition<TEntity> CreateEntitiesFilter(IEnumerable<TEntity> entities, bool applyFilters = false);

        /// <summary>
        /// Creates filter for given ids.
        /// </summary>
        /// <remarks>
        /// Visit https://docs.mongodb.com/manual/reference/operator/query/in/ to get more information about 'in' operator.
        /// </remarks>
        /// <param name="ids">Entity Ids to be filtered.</param>
        /// <param name="applyFilters">Set true to use GlobalFilters. Default is false.</param>
        FilterDefinition<TEntity> CreateEntitiesFilter(IEnumerable<TKey> ids, bool applyFilters = false);
    }
}