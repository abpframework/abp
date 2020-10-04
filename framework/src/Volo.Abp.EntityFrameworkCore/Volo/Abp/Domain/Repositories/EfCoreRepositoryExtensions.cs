using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace Volo.Abp.Domain.Repositories
{
    public static class EfCoreRepositoryExtensions
    {
        public static DbContext GetDbContext<TEntity, TKey>(this IReadOnlyBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToEfCoreRepository().DbContext;
        }

        public static DbContext GetDbContext<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            return repository.ToEfCoreRepository().DbContext;
        }

        public static DbSet<TEntity> GetDbSet<TEntity, TKey>(this IReadOnlyBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            return repository.ToEfCoreRepository().DbSet;
        }

        public static DbSet<TEntity> GetDbSet<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            return repository.ToEfCoreRepository().DbSet;
        }

        public static IEfCoreRepository<TEntity, TKey> ToEfCoreRepository<TEntity, TKey>(this IReadOnlyBasicRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            if (!(repository is IEfCoreRepository<TEntity, TKey> efCoreRepository))
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IEfCoreRepository<TEntity, TKey>).AssemblyQualifiedName, nameof(repository));
            }

            return efCoreRepository;
        }

        public static IEfCoreRepository<TEntity> ToEfCoreRepository<TEntity>(
            this IReadOnlyBasicRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            if (!(repository is IEfCoreRepository<TEntity> efCoreRepository))
            {
                throw new ArgumentException("Given repository does not implement " + typeof(IEfCoreRepository<TEntity>).AssemblyQualifiedName, nameof(repository));
            }
            return efCoreRepository;
        }
    }
}
