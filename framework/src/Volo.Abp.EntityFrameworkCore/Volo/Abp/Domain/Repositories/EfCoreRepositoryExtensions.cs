using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace Volo.Abp.Domain.Repositories
{
    public static class EfCoreRepositoryExtensions
    {
        public static DbContext GetDbContext<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            return repository.ToEfCoreRepository().DbContext;
        }

        public static DbSet<TEntity> GetDbSet<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            return repository.ToEfCoreRepository().DbSet;
        }

        public static IEfCoreRepository<TEntity> ToEfCoreRepository<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            if (repository is IEfCoreRepository<TEntity> efCoreRepository)
            {
                return efCoreRepository;
            }

            throw new ArgumentException("Given repository does not implement " + typeof(IEfCoreRepository<TEntity>).AssemblyQualifiedName, nameof(repository));
        }
    }
}
