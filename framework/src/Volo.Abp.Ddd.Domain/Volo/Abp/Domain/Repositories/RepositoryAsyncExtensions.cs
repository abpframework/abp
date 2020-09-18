using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public static class RepositoryAsyncExtensions
    {
        #region Count/LongCount

        public static Task<int> CountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.CountAsync(
                repository,
                cancellationToken
            );
        }

        public static Task<int> CountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.CountAsync(
                repository,
                predicate,
                cancellationToken
            );
        }

        #endregion
    }
}
