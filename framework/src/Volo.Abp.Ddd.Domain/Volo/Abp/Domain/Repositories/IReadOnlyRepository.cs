using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Linq;

namespace Volo.Abp.Domain.Repositories
{
    public interface IReadOnlyRepository<TEntity> : IQueryable<TEntity>, IReadOnlyBasicRepository<TEntity>
        where TEntity : class, IEntity
    {
        IAsyncQueryableExecuter AsyncExecuter { get; }

        [Obsolete("Use WithDetailsAsync method.")]
        IQueryable<TEntity> WithDetails();

        [Obsolete("Use WithDetailsAsync method.")]
        IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors);

        Task<IQueryable<TEntity>> WithDetailsAsync(); //TODO: CancellationToken

        Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors); //TODO: CancellationToken

        Task<IQueryable<TEntity>> GetQueryableAsync(); //TODO: CancellationToken
    }

    public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
