using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public interface IMongoDbRepository<TEntity> : IQueryableRepository<TEntity>
        where TEntity : class, IEntity
    {
        IMongoDatabase Database { get; }

        IMongoCollection<TEntity> Collection { get; }

        string CollectionName { get; }
    }

    public interface IMongoDbRepository<TEntity, TPrimaryKey> : IMongoDbRepository<TEntity>, IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

    }
}
