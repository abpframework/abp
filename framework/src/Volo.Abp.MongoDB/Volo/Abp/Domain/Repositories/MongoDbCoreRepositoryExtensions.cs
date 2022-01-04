using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

    [Obsolete("Use GetMongoQueryableAsync method.")]
    public static IMongoQueryable<TEntity> GetMongoQueryable<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetMongoQueryable();
    }

    public static Task<IMongoQueryable<TEntity>> GetMongoQueryableAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetMongoQueryableAsync(cancellationToken);
    }

    public static Task<IAggregateFluent<TEntity>> GetAggregateAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        return repository.ToMongoDbRepository().GetAggregateAsync(cancellationToken);
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
