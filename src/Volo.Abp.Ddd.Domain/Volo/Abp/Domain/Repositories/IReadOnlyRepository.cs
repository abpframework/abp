using System.Linq;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public interface IReadOnlyRepository<TEntity> : IQueryable<TEntity>
    {

    }

    public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
