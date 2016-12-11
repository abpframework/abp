using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    public interface IEfCoreRepository
    {
        DbContext DbContext { get; }
    }
    
    public interface IEfCoreRepository<TEntity, TPrimaryKey> : IEfCoreRepository
        where TEntity : class, IEntity<TPrimaryKey>
    {
        DbSet<TEntity> DbSet { get; }
    }
}