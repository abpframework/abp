using System.Collections.Generic;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public interface IMemoryDatabase
    {
        List<TEntity> Collection<TEntity>();

        TKey GenerateNextId<TEntity, TKey>();
    }
}