using System;
using System.Threading;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public class AbpEfCoreCurrentDbContext
{
    private readonly AsyncLocal<IAbpEfCoreDbFunctionContext?> _current = new AsyncLocal<IAbpEfCoreDbFunctionContext?>();

    public IAbpEfCoreDbFunctionContext? Context => _current.Value;

    // The filter state is read while EF Core translates the query, and only AbpEntityQueryProvider sets the current DbContext for that.
    // Queries compiled without it, such as EF.CompileQuery, must fail instead of running without the data filters.
    public virtual IAbpEfCoreDbFunctionContext GetRequiredContext()
    {
        return Context ?? throw new AbpException(
            "The data filters cannot be applied to a query that is compiled outside of AbpEntityQueryProvider, such as EF.CompileQuery or EF.CompileAsyncQuery. " +
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
}
