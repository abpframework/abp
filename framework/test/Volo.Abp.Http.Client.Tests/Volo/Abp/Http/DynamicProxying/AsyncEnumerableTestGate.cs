using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Http.DynamicProxying;

public static class AsyncEnumerableTestGate
{
    private static TaskCompletionSource<bool> _gate = Create();

    public static void Reset()
    {
        _gate = Create();
    }

    public static void Open()
    {
        _gate.TrySetResult(true);
    }

    public static Task WaitAsync(CancellationToken cancellationToken)
    {
        return _gate.Task.WaitAsync(cancellationToken);
    }

    private static TaskCompletionSource<bool> Create()
    {
        return new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
    }
}
