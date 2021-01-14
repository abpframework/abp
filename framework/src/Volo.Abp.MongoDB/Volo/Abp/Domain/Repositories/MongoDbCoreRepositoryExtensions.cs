using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.Domain.Repositories
{
    public static class MongoDbCoreRepositoryExtensions
    {
        [Obsolete("Use GetDatabaseAsync method.")]
        public static IMongoDatabase GetDatabase<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().Database;
        }

        public static Task<IMongoDatabase> GetDatabaseAsync<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().GetDatabaseAsync();
        }

        [Obsolete("Use GetCollectionAsync method.")]
        public static IMongoCollection<TEntity> GetCollection<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().Collection;
        }

        public static Task<IMongoCollection<TEntity>> GetCollectionAsync<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().GetCollectionAsync();
        }

        [Obsolete("Use GetMongoQueryableAsync method.")]
        public static IMongoQueryable<TEntity> GetMongoQueryable<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().GetMongoQueryable();
        }

        public static Task<IMongoQueryable<TEntity>> GetMongoQueryableAsync<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMongoDbRepository().GetMongoQueryableAsync();
        }

        public static IMongoDbRepository<TEntity, TKey> ToMongoDbRepository<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
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
