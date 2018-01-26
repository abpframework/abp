using System;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.Domain.Repositories
{
    public static class MongoDbCoreRepositoryExtensions
    {
        public static IMongoDatabase GetDatabase<TEntity, TKey>(this IRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().Database;
        }

        public static IMongoCollection<TEntity> GetCollection<TEntity, TKey>(this IRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().Collection;
        }

        public static string GetCollectionName<TEntity, TKey>(this IRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().CollectionName;
        }

        public static IMongoDbRepository<TEntity, TKey> ToMongoDbRepository<TEntity, TKey>(this IRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            var mongoDbRepository = repository as IMongoDbRepository<TEntity, TKey>;
            if (mongoDbRepository == null)
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IMongoDbRepository<TEntity, TKey>).AssemblyQualifiedName, nameof(repository));
            }

            return mongoDbRepository;
        }
    }
}