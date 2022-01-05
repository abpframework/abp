using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.MongoDB;

public class MongoDbAsyncQueryableProvider : IAsyncQueryableProvider, ISingletonDependency
{
    public bool CanExecute<T>(IQueryable<T> queryable)
    {
        return ProxyHelper.UnProxy(queryable) is IMongoQueryable<T>;
    }

    protected virtual IMongoQueryable<T> GetMongoQueryable<T>(IQueryable<T> queryable)
    {
        return ProxyHelper.UnProxy(queryable).As<IMongoQueryable<T>>();
    }

    public Task<bool> ContainsAsync<T>(IQueryable<T> queryable, T item, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMongoQueryable(queryable).Contains(item));
    }

    public Task<bool> AnyAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AnyAsync(cancellationToken);
    }

    public Task<bool> AnyAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AnyAsync(predicate, cancellationToken);
    }

    public Task<bool> AllAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMongoQueryable(queryable).All(predicate));
    }

    public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).CountAsync(cancellationToken);
    }

    public Task<int> CountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).CountAsync(predicate, cancellationToken);
    }

    public Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).LongCountAsync(cancellationToken);
    }

    public Task<long> LongCountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).LongCountAsync(predicate, cancellationToken);
    }

    public Task<T> FirstAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).FirstAsync(cancellationToken);
    }

    public Task<T> FirstAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).FirstAsync(predicate, cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> LastAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMongoQueryable(queryable).Last());
    }

    public Task<T> LastAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMongoQueryable(queryable).Last(predicate));
    }

    public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMongoQueryable(queryable).LastOrDefault());
    }

    public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetMongoQueryable(queryable).LastOrDefault(predicate));
    }

    public Task<T> SingleAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SingleAsync(cancellationToken);
    }

    public Task<T> SingleAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SingleAsync(predicate, cancellationToken);
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SingleOrDefaultAsync(cancellationToken);
    }

    public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<T> MinAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).MinAsync(cancellationToken);
    }

    public Task<TResult> MinAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).MinAsync(selector, cancellationToken);
    }

    public Task<T> MaxAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).MaxAsync(cancellationToken);
    }

    public Task<TResult> MaxAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).MaxAsync(selector, cancellationToken);
    }

    public Task<decimal> SumAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<decimal?> SumAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<decimal> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<decimal?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<int> SumAsync(IQueryable<int> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<int?> SumAsync(IQueryable<int?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<int> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<int?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<long> SumAsync(IQueryable<long> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<long?> SumAsync(IQueryable<long?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<long> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<long?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<double> SumAsync(IQueryable<double> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<double?> SumAsync(IQueryable<double?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<double> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<double?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<float> SumAsync(IQueryable<float> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<float?> SumAsync(IQueryable<float?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(cancellationToken);
    }

    public Task<float> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<float?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).SumAsync(selector, cancellationToken);
    }

    public Task<decimal> AverageAsync(IQueryable<decimal> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<decimal?> AverageAsync(IQueryable<decimal?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<decimal> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<decimal?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync(IQueryable<int> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync(IQueryable<int?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync(IQueryable<long> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync(IQueryable<long?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<double> AverageAsync(IQueryable<double> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<double?> AverageAsync(IQueryable<double?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<float> AverageAsync(IQueryable<float> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<float?> AverageAsync(IQueryable<float?> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(cancellationToken);
    }

    public Task<float> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<float?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).AverageAsync(selector, cancellationToken);
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return GetMongoQueryable(queryable).ToListAsync(cancellationToken);
    }

    public async Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return (await GetMongoQueryable(queryable).ToListAsync(cancellationToken)).ToArray();
    }
}
