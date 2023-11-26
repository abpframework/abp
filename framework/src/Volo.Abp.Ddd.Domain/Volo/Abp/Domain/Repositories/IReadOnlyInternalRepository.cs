using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories;

public interface IReadOnlyInternalRepository<TEntity> where TEntity : class, IEntity
{
}