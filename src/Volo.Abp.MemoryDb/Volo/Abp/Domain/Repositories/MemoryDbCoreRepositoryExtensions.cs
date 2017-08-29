using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MemoryDb;

namespace Volo.Abp.Domain.Repositories
{
    public static class MemoryDbCoreRepositoryExtensions
    {
        public static IMemoryDatabase GetDatabase<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.ToMemoryDbRepository().Database;
        }

        public static List<TEntity> GetCollection<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return repository.ToMemoryDbRepository().Collection;
        }

        public static IMemoryDbRepository<TEntity, TPrimaryKey> ToMemoryDbRepository<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var memoryDbRepository = repository as IMemoryDbRepository<TEntity, TPrimaryKey>;
            if (memoryDbRepository == null)
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IMemoryDbRepository<TEntity, TPrimaryKey>).AssemblyQualifiedName, nameof(repository));
            }

            return memoryDbRepository;
        }
    }
}