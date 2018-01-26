using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public interface IMongoDbRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        IMongoDatabase Database { get; }

        IMongoCollection<TEntity> Collection { get; }

        string CollectionName { get; }
    }

    public interface IMongoDbRepository<TEntity, TKey> : IMongoDbRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
