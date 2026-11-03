using System;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

#pragma warning disable EF1001
// A custom query compiler that an application registers with ReplaceService.
public class CompiledQueryCacheTestQueryCompiler : IQueryCompiler
{
    private readonly QueryCompiler _inner;

    public int ExecuteCount { get; private set; }

    public int CreateCompiledQueryCount { get; private set; }

    public CompiledQueryCacheTestQueryCompiler(IServiceProvider serviceProvider)
    {
        _inner = ActivatorUtilities.CreateInstance<QueryCompiler>(serviceProvider);
    }

    public TResult Execute<TResult>(Expression query)
    {
        ExecuteCount++;
        return _inner.Execute<TResult>(query);
    }

    public TResult ExecuteAsync<TResult>(Expression query, CancellationToken cancellationToken)
    {
        ExecuteCount++;
        return _inner.ExecuteAsync<TResult>(query, cancellationToken);
    }

    public Func<QueryContext, TResult> CreateCompiledQuery<TResult>(Expression query)
    {
        CreateCompiledQueryCount++;
        return _inner.CreateCompiledQuery<TResult>(query);
    }

    public Func<QueryContext, TResult> CreateCompiledAsyncQuery<TResult>(Expression query)
    {
        CreateCompiledQueryCount++;
        return _inner.CreateCompiledAsyncQuery<TResult>(query);
    }

#pragma warning disable EF9100
    public Expression<Func<QueryContext, TResult>> PrecompileQuery<TResult>(Expression query, bool async)
    {
        return _inner.PrecompileQuery<TResult>(query, async);
    }
#pragma warning restore EF9100
}
#pragma warning restore EF1001
