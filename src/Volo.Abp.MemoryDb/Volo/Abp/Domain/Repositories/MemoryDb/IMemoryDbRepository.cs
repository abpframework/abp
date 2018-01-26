using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public interface IMemoryDbRepository<TEntity> : IQueryableRepository<TEntity>
        where TEntity : class, IEntity
    {
        IMemoryDatabase Database { get; }

        List<TEntity> Collection { get; }
    }

    public interface IMemoryDbRepository<TEntity, TKey> : IMemoryDbRepository<TEntity>, IQueryableRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
