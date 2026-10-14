using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;

namespace Volo.Abp.EntityFrameworkCore;

#pragma warning disable EF1001
public class AbpQueryCompiler : QueryCompiler
{
    protected AbpEfCoreCurrentDbContext AbpEfCoreCurrentDbContext { get; }

    public AbpQueryCompiler(
        IQueryContextFactory queryContextFactory,
        ICompiledQueryCache compiledQueryCache,
        ICompiledQueryCacheKeyGenerator compiledQueryCacheKeyGenerator,
        IDatabase database,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger,
        ICurrentDbContext currentContext,
        IEvaluatableExpressionFilter evaluatableExpressionFilter,
        IModel model,
        AbpEfCoreCurrentDbContext abpEfCoreCurrentDbContext)
        : base(
            queryContextFactory,
            compiledQueryCache,
            compiledQueryCacheKeyGenerator,
            database,
            logger,
            currentContext,
            evaluatableExpressionFilter,
            model)
    {
        AbpEfCoreCurrentDbContext = abpEfCoreCurrentDbContext;
    }

    // EF.CompileQuery translates the query once and reuses it for every later run, so the filter state must never be read for it.
    // The first run can happen while an outer query is being executed, for example when the compiled query is invoked inside its expression,
    // so the current DbContext of that outer query is cleared here.
    public override Func<QueryContext, TResult> CreateCompiledQuery<TResult>(Expression query)
    {
        using (AbpEfCoreCurrentDbContext.Use(null))
        {
            return base.CreateCompiledQuery<TResult>(query);
        }
    }

    public override Func<QueryContext, TResult> CreateCompiledAsyncQuery<TResult>(Expression query)
    {
        using (AbpEfCoreCurrentDbContext.Use(null))
        {
            return base.CreateCompiledAsyncQuery<TResult>(query);
        }
    }
}
#pragma warning restore EF1001
