using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB;

public interface IMongoDbRepositoryFilterer<TEntity> where TEntity : class, IEntity
{
    Task AddGlobalFiltersAsync(List<FilterDefinition<TEntity>> filters);
}

public interface IMongoDbRepositoryFilterer<TEntity, TKey> : IMongoDbRepositoryFilterer<TEntity> where TEntity : class, IEntity<TKey>
{
    Task<FilterDefinition<TEntity>> CreateEntityFilterAsync(TKey id, bool applyFilters = false);

    Task<FilterDefinition<TEntity>> CreateEntityFilterAsync(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null);

    /// <summary>
    /// Creates filter for given entities.
    /// </summary>
    /// <remarks>
    /// Visit https://docs.mongodb.com/manual/reference/operator/query/in/ to get more information about 'in' operator.
    /// </remarks>
    /// <param name="entities">Entities to be filtered.</param>
    /// <param name="applyFilters">Set true to use GlobalFilters. Default is false.</param>
    /// <returns>Created <see cref="FilterDefinition{TDocument}"/>.</returns>
    Task<FilterDefinition<TEntity>> CreateEntitiesFilterAsync(IEnumerable<TEntity> entities, bool applyFilters = false);

    /// <summary>
    /// Creates filter for given ids.
    /// </summary>
    /// <remarks>
    /// Visit https://docs.mongodb.com/manual/reference/operator/query/in/ to get more information about 'in' operator.
    /// </remarks>
    /// <param name="ids">Entity Ids to be filtered.</param>
    /// <param name="applyFilters">Set true to use GlobalFilters. Default is false.</param>
    Task<FilterDefinition<TEntity>> CreateEntitiesFilterAsync(IEnumerable<TKey> ids, bool applyFilters = false);
}
