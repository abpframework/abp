using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;

namespace Volo.Abp.EntityFrameworkCore;

public class EfCoreAsyncQueryableProvider : IAsyncQueryableProvider, ISingletonDependency
{
    public bool CanExecute<T>(IQueryable<T> queryable)
    {
        return queryable.Provider is EntityQueryProvider;
    }

    public Task<bool> ContainsAsync<T>(IQueryable<T> queryable, T item, CancellationToken cancellationToken = default)
    {
        return queryable.ContainsAsync(item, cancellationToken);
    }

    public Task<bool> AnyAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AnyAsync(cancellationToken);
    }

    public Task<bool> AnyAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.AnyAsync(predicate, cancellationToken);
    }

    public Task<bool> AllAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.AllAsync(predicate, cancellationToken);
    }

    public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.CountAsync(cancellationToken);
    }

    public Task<int> CountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.CountAsync(predicate, cancellationToken);
    }

    public Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.LongCountAsync(cancellationToken);
    }

    public Task<long> LongCountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.LongCountAsync(predicate, cancellationToken);
    }

    public Task<T> FirstAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.FirstAsync(cancellationToken);
    }

    public Task<T> FirstAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.FirstAsync(predicate, cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> LastAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.LastAsync(cancellationToken);
    }

    public Task<T> LastAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.LastAsync(predicate, cancellationToken);
    }

    public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.LastOrDefaultAsync(cancellationToken);
    }

    public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.LastOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> SingleAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SingleAsync(cancellationToken);
    }

    public Task<T> SingleAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return queryable.SingleAsync(predicate, cancellationToken);
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SingleOrDefaultAsync(cancellationToken);
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return queryable.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> MinAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.MinAsync(cancellationToken);
    }

    public Task<TResult> MinAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.MinAsync(selector, cancellationToken);
    }

    public Task<T> MaxAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.MaxAsync(cancellationToken);
    }

    public Task<TResult> MaxAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.MaxAsync(selector, cancellationToken);
    }

    public Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<decimal?> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<decimal> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<decimal?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<int?> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<int> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<int?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<long?> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<long> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<long?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<double?> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<double> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<double?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<float?> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(cancellationToken);
    }

    public Task<float> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<float?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.SumAsync(selector, cancellationToken);
    }

    public Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<decimal?> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<decimal> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<decimal?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<float?> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(cancellationToken);
    }

    public Task<float> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<float?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
    {
        return queryable.AverageAsync(selector, cancellationToken);
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.ToListAsync(cancellationToken);
    }

    public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return queryable.ToArrayAsync(cancellationToken);
    }
}
