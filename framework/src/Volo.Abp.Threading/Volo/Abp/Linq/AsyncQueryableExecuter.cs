using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Linq
{
    public class AsyncQueryableExecuter : IAsyncQueryableExecuter, ISingletonDependency
    {
        protected IEnumerable<IAsyncQueryableProvider> Providers { get; }

        public AsyncQueryableExecuter(IEnumerable<IAsyncQueryableProvider> providers)
        {
            Providers = providers;
        }

        protected virtual IAsyncQueryableProvider FindProvider<T>(IQueryable<T> queryable)
        {
            return Providers.FirstOrDefault(p => p.CanExecute(queryable));
        }

        public Task<bool> ContainsAsync<T>(IQueryable<T> queryable, T item, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.ContainsAsync(queryable, item, cancellationToken)
                : Task.FromResult(queryable.Contains(item));
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AnyAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.Any(predicate));
        }

        public Task<bool> AllAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AllAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.All(predicate));
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.CountAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Count());
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.CountAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.Count(predicate));
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.LongCountAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.LongCount());
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.LongCountAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.LongCount(predicate));
        }

        public Task<T> FirstAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.FirstAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.First());
        }

        public Task<T> FirstAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.FirstAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.First(predicate));
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.FirstOrDefaultAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.FirstOrDefault());
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.FirstOrDefaultAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.FirstOrDefault(predicate));
        }

        public Task<T> LastAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.LastAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Last());
        }

        public Task<T> LastAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.LastAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.Last(predicate));
        }

        public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.LastOrDefaultAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.LastOrDefault());
        }

        public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.LastOrDefaultAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.LastOrDefault(predicate));
        }

        public Task<T> SingleAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SingleAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Single());
        }

        public Task<T> SingleAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SingleAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.Single(predicate));
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SingleOrDefaultAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.SingleOrDefault());
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SingleOrDefaultAsync(queryable, predicate, cancellationToken)
                : Task.FromResult(queryable.SingleOrDefault(predicate));
        }

        public Task<T> MinAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.MinAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Min());
        }

        public Task<TResult> MinAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.MinAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Min(selector));
        }

        public Task<T> MaxAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.MaxAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Max());
        }

        public Task<TResult> MaxAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.MaxAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Max(selector));
        }

        public Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<decimal?> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<decimal> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<decimal?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<int?> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<int> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<int?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<long?> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<long> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<long?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<double?> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<double> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<double?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<float?> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Sum());
        }

        public Task<float> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<float?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.SumAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Sum(selector));
        }

        public Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<decimal?> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<decimal> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<decimal?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<double?> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<double?> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<double?> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<float?> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.Average());
        }

        public Task<float> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<float?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.AverageAsync(queryable, selector, cancellationToken)
                : Task.FromResult(queryable.Average(selector));
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.ToListAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.ToList());
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            var provider = FindProvider(queryable);
            return provider != null
                ? provider.ToArrayAsync(queryable, cancellationToken)
                : Task.FromResult(queryable.ToArray());
        }
    }
}
