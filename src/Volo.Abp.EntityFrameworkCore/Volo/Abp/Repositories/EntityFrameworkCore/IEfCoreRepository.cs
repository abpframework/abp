using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    public interface IEfCoreRepository<TEntity, TPrimaryKey> : IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        DbContext DbContext { get; }

        DbSet<TEntity> DbSet { get; }
    }
}