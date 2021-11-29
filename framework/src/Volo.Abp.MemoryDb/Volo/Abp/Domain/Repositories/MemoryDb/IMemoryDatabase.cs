using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public interface IMemoryDatabase
    {
        IMemoryDatabaseCollection<TEntity> Collection<TEntity>() where TEntity : class, IEntity;

        TKey GenerateNextId<TEntity, TKey>();
    }
}