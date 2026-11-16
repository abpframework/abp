using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableGatedTestObject
{
    public virtual async IAsyncEnumerable<int> GetItemsAsync()
    {
        await Task.Yield();
        yield return 1;
    }

    public virtual async IAsyncEnumerable<string?> GetCallerValueAsync()
    {
        await Task.Yield();
        yield return AsyncEnumerableCallerContext.Value.Value;
    }
}
