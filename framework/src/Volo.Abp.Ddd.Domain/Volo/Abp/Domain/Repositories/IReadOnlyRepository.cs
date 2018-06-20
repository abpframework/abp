using System;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public interface IReadOnlyRepository<TEntity> : IQueryable<TEntity>, IReadOnlyBasicRepository<TEntity>
        where TEntity : class, IEntity
    {
        IQueryable<TEntity> WithDetails();

        IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors);
    }

    public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
