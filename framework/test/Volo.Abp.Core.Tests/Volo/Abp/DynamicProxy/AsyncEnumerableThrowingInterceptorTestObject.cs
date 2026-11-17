using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableThrowingInterceptorTestObject
{
    public bool Invoked { get; private set; }

    public virtual IAsyncEnumerable<int> GetItemsAsync()
    {
        Invoked = true;
        return AsyncEnumerable.Empty<int>();
    }
}
