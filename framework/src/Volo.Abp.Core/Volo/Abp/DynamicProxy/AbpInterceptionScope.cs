using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AbpInterceptionScope : IAbpInterceptionScope
{
    private readonly IDisposable? _disposable;

    public AbpInterceptionScope(IDisposable? disposable = null)
    {
        _disposable = disposable;
    }

    public virtual Task CompleteAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public virtual Task FailAsync(Exception exception)
    {
        return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        _disposable?.Dispose();
    }
}
