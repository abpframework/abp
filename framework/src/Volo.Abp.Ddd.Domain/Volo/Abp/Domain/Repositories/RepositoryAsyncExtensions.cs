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

        public static async Task<bool> ContainsAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] T item,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.ContainsAsync(queryable, item, cancellationToken);
        }

        #endregion

        #region Any/All

        public static async Task<bool> AnyAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AnyAsync(queryable, predicate, cancellationToken);
        }

        public static async Task<bool> AllAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AllAsync(queryable, predicate, cancellationToken);
        }

        #endregion

        #region Count/LongCount

        public static async Task<int> CountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.CountAsync(queryable, cancellationToken);
        }

        public static async Task<int> CountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.CountAsync(queryable, predicate, cancellationToken);
        }

        public static async Task<long> LongCountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.LongCountAsync(queryable, cancellationToken);
        }

        public static async Task<long> LongCountAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.LongCountAsync(queryable, predicate, cancellationToken);
        }

        #endregion

        #region First/FirstOrDefault

        public static async Task<T> FirstAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.FirstAsync(queryable, cancellationToken);
        }

        public static async Task<T> FirstAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.FirstAsync(queryable, predicate, cancellationToken);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.FirstOrDefaultAsync(queryable, cancellationToken);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.FirstOrDefaultAsync(queryable, predicate, cancellationToken);
        }

        #endregion

        #region Last/LastOrDefault

        public static async Task<T> LastAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.LastAsync(queryable, cancellationToken);
        }

        public static async Task<T> LastAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.LastAsync(queryable, predicate, cancellationToken);
        }

        public static async Task<T> LastOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.LastOrDefaultAsync(queryable, cancellationToken);
        }

        public static async Task<T> LastOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.LastOrDefaultAsync(queryable, predicate, cancellationToken);
        }

        #endregion

        #region Single/SingleOrDefault

        public static async Task<T> SingleAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SingleAsync(queryable, cancellationToken);
        }

        public static async Task<T> SingleAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SingleAsync(queryable, predicate, cancellationToken);
        }

        public static async Task<T> SingleOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SingleOrDefaultAsync(queryable, cancellationToken);
        }

        public static async Task<T> SingleOrDefaultAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SingleOrDefaultAsync(queryable, predicate, cancellationToken);
        }

        #endregion

        #region Min

        public static async Task<T> MinAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.MinAsync(queryable, cancellationToken);
        }

        public static async Task<TResult> MinAsync<T, TResult>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.MinAsync(queryable, selector, cancellationToken);
        }

        #endregion

        #region Max

        public static async Task<T> MaxAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.MaxAsync(queryable, cancellationToken);
        }

        public static async Task<TResult> MaxAsync<T, TResult>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.MaxAsync(queryable, selector, cancellationToken);
        }

        #endregion

        #region Sum

        public static async Task<decimal> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<decimal?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<int> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<int?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<long> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<long?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<float> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, float>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        public static async Task<float?> SumAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, float?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.SumAsync(queryable, selector, cancellationToken);
        }

        #endregion

        #region Average

        public static async Task<decimal> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<decimal?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, decimal?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, int?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, long?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<double?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, double?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        public static async Task<float?> AverageAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            [NotNull] Expression<Func<T, float?>> selector,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.AverageAsync(queryable, selector, cancellationToken);
        }

        #endregion

        #region ToList/Array

        public static async Task<List<T>> ToListAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.ToListAsync(queryable, cancellationToken);
        }

        public static async Task<T[]> ToArrayAsync<T>(
            [NotNull] this IReadOnlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var queryable = await repository.GetQueryableAsync();
            return await repository.AsyncExecuter.ToArrayAsync(queryable, cancellationToken);
        }

        #endregion
    }
}
