using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableConcurrentScopedTestObject
{
    public ConcurrentQueue<string> Events { get; } = new();

    public virtual async IAsyncEnumerable<string?> GetScopeIdsAsync(Task gate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        yield return ConcurrentScopeTestInterceptor.ScopeId.Value;
        await gate.WaitAsync(cancellationToken);
        yield return ConcurrentScopeTestInterceptor.ScopeId.Value;
    }
}
