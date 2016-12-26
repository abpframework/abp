using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public interface IMongoDbRepository<TEntity> : IMongoDbRepository<TEntity, string>, IQueryableRepository<TEntity>
        where TEntity : class, IEntity<string>
    {
        
    }

    public interface IMongoDbRepository<TEntity, TPrimaryKey> : IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
    }

    public interface IMongoDatabaseProvider
    {
        IMongoDatabase GetDatabase();
    }
}
