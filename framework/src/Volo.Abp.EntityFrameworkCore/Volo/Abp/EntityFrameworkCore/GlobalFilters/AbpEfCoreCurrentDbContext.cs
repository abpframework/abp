using System;
using System.Threading;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public class AbpEfCoreCurrentDbContext
{
    private readonly AsyncLocal<IAbpEfCoreDbFunctionContext?> _current = new AsyncLocal<IAbpEfCoreDbFunctionContext?>();

    public IAbpEfCoreDbFunctionContext? Context => _current.Value;

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
