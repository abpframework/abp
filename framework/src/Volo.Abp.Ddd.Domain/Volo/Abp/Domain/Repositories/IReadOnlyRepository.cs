using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
        
        /// <summary>
        /// Gets a list entities by the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to find the entity</param>
        /// <param name="includeDetails">Set true to include all children of this entity</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Entity</returns>
        Task<List<TEntity>> GetListAsync(
            [NotNull] Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }

    public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
