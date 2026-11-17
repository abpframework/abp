using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableDecoratedTestObject
{
    public virtual async IAsyncEnumerable<int> GetItemsAsync()
    {
        await Task.Yield();
        yield return 1;
        yield return 2;
    }
}
