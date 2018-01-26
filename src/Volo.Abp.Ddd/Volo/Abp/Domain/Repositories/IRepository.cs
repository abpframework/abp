using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    /// <summary>
    /// Just to mark a class as repository.
    /// </summary>
    public interface IRepository : ITransientDependency
    {

    }

    public interface IRepository<TEntity> : IBasicRepository<TEntity>, IQueryable<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void Delete([NotNull] Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }

    public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
    }
}