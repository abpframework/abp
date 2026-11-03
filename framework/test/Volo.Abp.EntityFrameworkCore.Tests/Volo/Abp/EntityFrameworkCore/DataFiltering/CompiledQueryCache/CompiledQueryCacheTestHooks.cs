using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheTestHooks
{
    public Action<CompiledQueryCacheTestDbContext> OnQueryCompilationStarting { get; set; }

    public Action OnGetCompiledQueryCacheKey { get; set; }
}

public class CompiledQueryCacheTestInterceptor : IQueryExpressionInterceptor
{
    private readonly CompiledQueryCacheTestHooks _hooks;

    public CompiledQueryCacheTestInterceptor(CompiledQueryCacheTestHooks hooks)
    {
        _hooks = hooks;
    }

    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        if (eventData.Context is CompiledQueryCacheTestDbContext dbContext)
        {
            _hooks.OnQueryCompilationStarting?.Invoke(dbContext);
        }

        return queryExpression;
    }
}

public class CompiledQueryCacheTestKeyProvider : IAbpEfCoreCompiledQueryCacheKeyProvider
{
    private readonly CompiledQueryCacheTestHooks _hooks;

    public CompiledQueryCacheTestKeyProvider(CompiledQueryCacheTestHooks hooks)
    {
        _hooks = hooks;
    }

    public string GetCompiledQueryCacheKey()
    {
        _hooks.OnGetCompiledQueryCacheKey?.Invoke();
        return null;
    }
}
