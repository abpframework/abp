using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Linq
{
    public interface IAsyncQueryableProvider
    {
        bool CanExecute<T>(IQueryable<T> queryable);

        #region Contains

        Task<bool> ContainsAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] T item,
            CancellationToken cancellationToken = default);

        #endregion

        #region Any/All

        Task<bool> AnyAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AllAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        #endregion

        #region Count/LongCount

        Task<int> CountAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<int> CountAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);


        Task<long> LongCountAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<long> LongCountAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        #endregion

        #region First/FirstOrDefault

        Task<T> FirstAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<T> FirstAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);


        Task<T> FirstOrDefaultAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<T> FirstOrDefaultAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        #endregion

        #region Last/LastOrDefault

        Task<T> LastAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<T> LastAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);


        Task<T> LastOrDefaultAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<T> LastOrDefaultAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        #endregion

        #region Single/SingleOrDefault

        Task<T> SingleAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<T> SingleAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<T> SingleOrDefaultAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);

        Task<T> SingleOrDefaultAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        #endregion

        #region Min

        Task<T> MinAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<TResult> MinAsync<T, TResult>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default);

        #endregion

        #region Max

        Task<T> MaxAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<TResult> MaxAsync<T, TResult>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, TResult>> selector,
            CancellationToken cancellationToken = default);

        #endregion

        #region Sum

        Task<decimal> SumAsync(
            [NotNull] IQueryable<decimal> source,
            CancellationToken cancellationToken = default);


        Task<decimal?> SumAsync(
            [NotNull] IQueryable<decimal?> source,
            CancellationToken cancellationToken = default);


        Task<decimal> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, decimal>> selector,
            CancellationToken cancellationToken = default);


        Task<decimal?> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, decimal?>> selector,
            CancellationToken cancellationToken = default);


        Task<int> SumAsync(
            [NotNull] IQueryable<int> source,
            CancellationToken cancellationToken = default);


        Task<int?> SumAsync(
            [NotNull] IQueryable<int?> source,
            CancellationToken cancellationToken = default);


        Task<int> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, int>> selector,
            CancellationToken cancellationToken = default);


        Task<int?> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, int?>> selector,
            CancellationToken cancellationToken = default);


        Task<long> SumAsync(
            [NotNull] IQueryable<long> source,
            CancellationToken cancellationToken = default);


        Task<long?> SumAsync(
            [NotNull] IQueryable<long?> source,
            CancellationToken cancellationToken = default);


        Task<long> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, long>> selector,
            CancellationToken cancellationToken = default);


        Task<long?> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, long?>> selector,
            CancellationToken cancellationToken = default);


        Task<double> SumAsync(
            [NotNull] IQueryable<double> source,
            CancellationToken cancellationToken = default);


        Task<double?> SumAsync(
            [NotNull] IQueryable<double?> source,
            CancellationToken cancellationToken = default);


        Task<double> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, double>> selector,
            CancellationToken cancellationToken = default);


        Task<double?> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, double?>> selector,
            CancellationToken cancellationToken = default);


        Task<float> SumAsync(
            [NotNull] IQueryable<float> source,
            CancellationToken cancellationToken = default);


        Task<float?> SumAsync(
            [NotNull] IQueryable<float?> source,
            CancellationToken cancellationToken = default);


        Task<float> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, float>> selector,
            CancellationToken cancellationToken = default);


        Task<float?> SumAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, float?>> selector,
            CancellationToken cancellationToken = default);

        #endregion

        #region Average

        Task<decimal> AverageAsync(
            [NotNull] IQueryable<decimal> source,
            CancellationToken cancellationToken = default);


        Task<decimal?> AverageAsync(
            [NotNull] IQueryable<decimal?> source,
            CancellationToken cancellationToken = default);


        Task<decimal> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, decimal>> selector,
            CancellationToken cancellationToken = default);

        Task<decimal?> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, decimal?>> selector,
            CancellationToken cancellationToken = default);


        Task<double> AverageAsync(
            [NotNull] IQueryable<int> source,
            CancellationToken cancellationToken = default);


        Task<double?> AverageAsync(
            [NotNull] IQueryable<int?> source,
            CancellationToken cancellationToken = default);


        Task<double> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, int>> selector,
            CancellationToken cancellationToken = default);

        Task<double?> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, int?>> selector,
            CancellationToken cancellationToken = default);

        Task<double> AverageAsync(
            [NotNull] IQueryable<long> source,
            CancellationToken cancellationToken = default);


        Task<double?> AverageAsync(
            [NotNull] IQueryable<long?> source,
            CancellationToken cancellationToken = default);


        Task<double> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, long>> selector,
            CancellationToken cancellationToken = default);


        Task<double?> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, long?>> selector,
            CancellationToken cancellationToken = default);


        Task<double> AverageAsync(
            [NotNull] IQueryable<double> source,
            CancellationToken cancellationToken = default);


        Task<double?> AverageAsync(
            [NotNull] IQueryable<double?> source,
            CancellationToken cancellationToken = default);


        Task<double> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, double>> selector,
            CancellationToken cancellationToken = default);


        Task<double?> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, double?>> selector,
            CancellationToken cancellationToken = default);


        Task<float> AverageAsync(
            [NotNull] IQueryable<float> source,
            CancellationToken cancellationToken = default);


        Task<float?> AverageAsync(
            [NotNull] IQueryable<float?> source,
            CancellationToken cancellationToken = default);


        Task<float> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, float>> selector,
            CancellationToken cancellationToken = default);


        Task<float?> AverageAsync<T>(
            [NotNull] IQueryable<T> queryable,
            [NotNull] Expression<Func<T, float?>> selector,
            CancellationToken cancellationToken = default);

        #endregion

        #region ToList/Array

        Task<List<T>> ToListAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);


        Task<T[]> ToArrayAsync<T>(
            [NotNull] IQueryable<T> queryable,
            CancellationToken cancellationToken = default);

        #endregion
    }
}
