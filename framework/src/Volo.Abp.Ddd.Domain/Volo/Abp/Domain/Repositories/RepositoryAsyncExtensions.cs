using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public static class RepositoryAsyncExtensions
    {
        #region Contains

        public static Task<bool> ContainsAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] T item,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.ContainsAsync(repository, item, cancellationToken);
        }

        #endregion

        #region Any/All

        public static Task<bool> AnyAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AnyAsync(repository, predicate, cancellationToken);
        }

        public static Task<bool> AllAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AllAsync(repository, predicate, cancellationToken);
        }

        #endregion

        #region Count/LongCount

        public static Task<int> CountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.CountAsync(repository, cancellationToken);
        }

        public static Task<int> CountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.CountAsync(repository, predicate, cancellationToken);
        }

        public static Task<long> LongCountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.LongCountAsync(repository, cancellationToken);
        }

        public static Task<long> LongCountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.LongCountAsync(repository, predicate, cancellationToken);
        }

        #endregion

        #region First/FirstOrDefault

        public static Task<T> FirstAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.FirstAsync(repository, cancellationToken);
        }

        public static Task<T> FirstAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.FirstAsync(repository, predicate, cancellationToken);
        }

        public static Task<T> FirstOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.FirstOrDefaultAsync(repository, cancellationToken);
        }

        public static Task<T> FirstOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.FirstOrDefaultAsync(repository, predicate, cancellationToken);
        }

        #endregion

        #region Last/LastOrDefault

        public static Task<T> LastAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.LastAsync(repository, cancellationToken);
        }

        public static Task<T> LastAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.LastAsync(repository, predicate, cancellationToken);
        }

        public static Task<T> LastOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.LastOrDefaultAsync(repository, cancellationToken);
        }

        public static Task<T> LastOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.LastOrDefaultAsync(repository, predicate, cancellationToken);
        }

        #endregion

        #region Single/SingleOrDefault

        public static Task<T> SingleAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SingleAsync(repository, cancellationToken);
        }

        public static Task<T> SingleAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SingleAsync(repository, predicate, cancellationToken);
        }

        public static Task<T> SingleOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SingleOrDefaultAsync(repository, cancellationToken);
        }

        public static Task<T> SingleOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SingleOrDefaultAsync(repository, predicate, cancellationToken);
        }

        #endregion

        #region Min

        public static Task<T> MinAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.MinAsync(repository, cancellationToken);
        }

        public static Task<TResult> MinAsync<T, TResult>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.MinAsync(repository, selector, cancellationToken);
        }

        #endregion

        #region Max

        public static Task<T> MaxAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.MaxAsync(repository, cancellationToken);
        }

        public static Task<TResult> MaxAsync<T, TResult>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.MaxAsync(repository, selector, cancellationToken);
        }

        #endregion

        #region Sum

        public static Task<decimal> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<decimal?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<int> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<int?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<long> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<long?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<double> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<double?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<float> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, float>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        public static Task<float?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, float?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.SumAsync(repository, selector, cancellationToken);
        }

        #endregion

        #region Average

        public static Task<decimal> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<decimal?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<double> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<double?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        public static Task<float?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, float?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.AverageAsync(repository, selector, cancellationToken);
        }

        #endregion

        #region ToList/Array

        public static Task<List<T>> ToListAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.ToListAsync(repository, cancellationToken);
        }

        public static Task<T[]> ToArrayAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            return repository.AsyncExecuter.ToArrayAsync(repository, cancellationToken);
        }

        #endregion
    }
}
