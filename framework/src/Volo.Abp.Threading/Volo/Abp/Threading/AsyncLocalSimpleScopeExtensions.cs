using System;
using System.Threading;

namespace Volo.Abp.Threading;

public static class AsyncLocalSimpleScopeExtensions
{
    public static IDisposable SetScoped<T>(this AsyncLocal<T> asyncLocal, T value)
    {
        var previousValue = asyncLocal.Value;
        asyncLocal.Value = value;
        return new DisposeAction(() =>
        {
            asyncLocal.Value = previousValue;
        });
    }
}
