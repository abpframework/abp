using System;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    //public interface IMongoDbRepository<TEntity> : IMongoDbRepository<TEntity, Guid>, IQueryableRepository<TEntity>
    //    where TEntity : class, IEntity<Guid>
    //{
        
    //}

    public interface IMongoDbRepository<TEntity, TPrimaryKey> : IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        IMongoDatabase Database { get; }

        IMongoCollection<TEntity> Collection { get; }

        string CollectionName { get; }
    }
}
