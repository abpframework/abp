using System;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.Domain.Repositories
{
    public static class MongoDbCoreRepositoryExtensions
    {
        public static IMongoDatabase GetDatabase<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.ToMongoDbRepository().Database;
        }

        public static IMongoCollection<TEntity> GetCollection<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.ToMongoDbRepository().Collection;
        }

        public static string GetCollectionName<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.ToMongoDbRepository().CollectionName;
        }

        public static IMongoDbRepository<TEntity, TPrimaryKey> ToMongoDbRepository<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var mongoDbRepository = repository as IMongoDbRepository<TEntity, TPrimaryKey>;
            if (mongoDbRepository == null)
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IMongoDbRepository<TEntity, TPrimaryKey>).AssemblyQualifiedName, nameof(repository));
            }

            return mongoDbRepository;
        }
    }
}