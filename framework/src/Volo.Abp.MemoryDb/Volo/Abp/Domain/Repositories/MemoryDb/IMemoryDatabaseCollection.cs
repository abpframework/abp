using System.Collections.Generic;

namespace Volo.Abp.Domain.Repositories.MemoryDb;

public interface IMemoryDatabaseCollection<TEntity> : IEnumerable<TEntity>
{
    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}
