using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableWrongReturnValueTestObject
{
    public virtual IAsyncEnumerable<int> GetItemsAsync()
    {
        return AsyncEnumerable.Empty<int>();
    }
}
