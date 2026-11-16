using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableScopedTestObject
{
    public List<string> Logs { get; } = new();

    public virtual async IAsyncEnumerable<string?> GetCallerValuesAsync()
    {
        await Task.Yield();
        yield return AsyncEnumerableCallerContext.Value.Value;
        await Task.Yield();
        yield return AsyncEnumerableCallerContext.Value.Value;
    }

    public virtual async IAsyncEnumerable<string?> GetScopeValuesAsync(bool fail)
    {
        await Task.Delay(1).ConfigureAwait(false);
        yield return ScopeTestInterceptor.ScopeValue.Value;
        await Task.Delay(1).ConfigureAwait(false);
        if (fail)
        {
            throw new InvalidOperationException("Failed in scope");
        }

        yield return ScopeTestInterceptor.ScopeValue.Value;
    }
}
