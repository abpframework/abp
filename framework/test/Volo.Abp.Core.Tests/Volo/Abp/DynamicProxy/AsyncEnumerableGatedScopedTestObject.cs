using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableGatedScopedTestObject : AsyncEnumerableScopedTestObject
{
    public virtual async IAsyncEnumerable<string> GetCallerAndScopeValuesAsync()
    {
        await Task.Yield();
        yield return $"{AsyncEnumerableCallerContext.Value.Value}:{ScopeTestInterceptor.ScopeValue.Value}";
        await Task.Yield();
        yield return $"{AsyncEnumerableCallerContext.Value.Value}:{ScopeTestInterceptor.ScopeValue.Value}";
    }
}
