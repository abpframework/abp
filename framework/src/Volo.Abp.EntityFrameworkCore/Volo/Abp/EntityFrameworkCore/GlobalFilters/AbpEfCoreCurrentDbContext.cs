using System;
using System.Linq.Expressions;
using System.Threading;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public class AbpEfCoreCurrentDbContext
{
    private readonly AsyncLocal<IAbpEfCoreDbFunctionContext?> _current = new AsyncLocal<IAbpEfCoreDbFunctionContext?>();
    private readonly AsyncLocal<QueryScope?> _queryScope = new AsyncLocal<QueryScope?>();

    public IAbpEfCoreDbFunctionContext? Context => _current.Value;

    public virtual IAbpEfCoreDbFunctionContext GetRequiredContext()
    {
        return Context ?? throw new AbpException(
            "The data filters cannot be applied to a query that is compiled outside of EF Core's normal query pipeline, such as EF.CompileQuery or EF.CompileAsyncQuery. " +
            "Run it as a normal LINQ query, or set 'UseDbFunction' of 'AbpEfCoreGlobalFilterOptions' to false.");
    }

    public IDisposable Use(IAbpEfCoreDbFunctionContext? context)
    {
        var previousValue = Context;
        _current.Value = context;
        return new DisposeAction(() =>
        {
            _current.Value = previousValue;
        });
    }

    // Only the query of AbpEntityQueryProvider puts the filter state into the compiled query cache key, so only its compilation
    // may read it. Other compilations, such as EF.CompileQuery or a query compiled while another query runs, must fail.
    internal IDisposable BeginQuery(IAbpEfCoreDbFunctionContext? context)
    {
        var previousScope = _queryScope.Value;
        _queryScope.Value = context == null ? null : new QueryScope(context);
        var contextScope = Use(context);
        return new DisposeAction(() =>
        {
            contextScope.Dispose();
            _queryScope.Value = previousScope;
        });
    }

    internal void AuthorizeQueryCompilation(IAbpEfCoreDbFunctionContext context, Expression query)
    {
        var scope = _queryScope.Value;
        if (scope != null && scope.Context == context)
        {
            scope.AuthorizedQuery = query;
        }
    }

    internal IDisposable BeginQueryCompilation(IAbpEfCoreDbFunctionContext? context, Expression query)
    {
        var scope = _queryScope.Value;
        if (scope == null || scope.Context != context || !ReferenceEquals(scope.AuthorizedQuery, query))
        {
            return Use(null);
        }

        scope.AuthorizedQuery = null;
        return Use(scope.Context);
    }

    private sealed class QueryScope
    {
        public IAbpEfCoreDbFunctionContext Context { get; }

        public Expression? AuthorizedQuery { get; set; }

        public QueryScope(IAbpEfCoreDbFunctionContext context)
        {
            Context = context;
        }
    }
}
