using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.Domain.Repositories;

public static class MongoDbCoreRepositoryExtensions
{
    [Obsolete("Use GetDatabaseAsync method.")]
    public static IMongoDatabase GetDatabase<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().Database;
    }

    public static Task<IMongoDatabase> GetDatabaseAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetDatabaseAsync(cancellationToken);
    }

    [Obsolete("Use GetCollectionAsync method.")]
    public static IMongoCollection<TEntity> GetCollection<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().Collection;
    }

    public static Task<IMongoCollection<TEntity>> GetCollectionAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetCollectionAsync(cancellationToken);
    }

    [Obsolete("Use GetQueryableAsync method.")]
    public static IQueryable<TEntity> GetMongoQueryable<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetMongoQueryable();
    }

    [Obsolete("Use GetQueryableAsync method.")]
    public static Task<IQueryable<TEntity>> GetMongoQueryableAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, CancellationToken cancellationToken = default, AggregateOptions? aggregateOptions = null)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetMongoQueryableAsync(cancellationToken, aggregateOptions);
    }

    public static Task<IQueryable<TEntity>> GetQueryableAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetQueryableAsync();
    }

    public static Task<IAggregateFluent<TEntity>> GetAggregateAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, CancellationToken cancellationToken = default, AggregateOptions? aggregateOptions = null)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetAggregateAsync(cancellationToken, aggregateOptions);
    }

    public static IMongoDbRepository<TEntity> ToMongoDbRepository<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        if (repository is IMongoDbRepository<TEntity> mongoDbRepository)
        {
            return mongoDbRepository;
        }
        throw new ArgumentException("Given repository does not implement " + typeof(IMongoDbRepository<TEntity>).AssemblyQualifiedName, nameof(repository));
    }
}
