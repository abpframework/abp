using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableConcurrentTestObject
{
    public virtual async IAsyncEnumerable<int> GetItemsAsync(int count)
    {
        for (var i = 1; i <= count; i++)
        {
            await Task.Yield();
            yield return i;
        }
    }
}
