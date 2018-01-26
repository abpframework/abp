using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.EntityFrameworkCore
{
    public interface IEfCoreRepository<TEntity> : IQueryableRepository<TEntity>
        where TEntity : class, IEntity
    {
        DbContext DbContext { get; }

        DbSet<TEntity> DbSet { get; }
    }

    public interface IEfCoreRepository<TEntity, TPrimaryKey> : IEfCoreRepository<TEntity>, IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

    }
}