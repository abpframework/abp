using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MemoryDb;

namespace Volo.Abp.Domain.Repositories
{
    public static class MemoryDbCoreRepositoryExtensions
    {
        [Obsolete("Use GetDatabaseAsync method.")]
        public static IMemoryDatabase GetDatabase<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMemoryDbRepository().Database;
        }

        public static Task<IMemoryDatabase> GetDatabaseAsync<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMemoryDbRepository().GetDatabaseAsync();
        }

        [Obsolete("Use GetCollectionAsync method.")]
        public static IMemoryDatabaseCollection<TEntity> GetCollection<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMemoryDbRepository().Collection;
        }

        public static Task<IMemoryDatabaseCollection<TEntity>> GetCollectionAsync<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToMemoryDbRepository().GetCollectionAsync();
        }

        public static IMemoryDbRepository<TEntity, TKey> ToMemoryDbRepository<TEntity, TKey>(this IBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            var memoryDbRepository = repository as IMemoryDbRepository<TEntity, TKey>;
            if (memoryDbRepository == null)
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IMemoryDbRepository<TEntity, TKey>).AssemblyQualifiedName, nameof(repository));
            }

            return memoryDbRepository;
        }
    }
}
