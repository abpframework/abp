using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.TestBase.Logging;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableInterceptionTargetClass : ICanLogOnObject
{
    private int _createAttempts;

    public List<string> Logs { get; } = new List<string>();

    public virtual async IAsyncEnumerable<int> GetItemsAsync(int count, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (var i = 1; i <= count; i++)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            Logs.Add($"Produce{i}");
            yield return i;
        }
    }

    public virtual async IAsyncEnumerable<int> GetOneThenWaitAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        yield return 1;
        await Task.Delay(Timeout.Infinite, cancellationToken);
        yield return 2;
    }

    public virtual async IAsyncEnumerable<int> GetOneThenThrowAsync()
    {
        await Task.Yield();
        yield return 1;
        throw new InvalidOperationException("Failed after first item");
    }

    public virtual IAsyncEnumerable<int> GetItemsCreatedOnSecondAttempt()
    {
        if (Interlocked.Increment(ref _createAttempts) == 1)
        {
            throw new InvalidOperationException("First attempt failed");
        }

        return GetItemsAsync(1);
    }

    public virtual IAsyncEnumerable<int> GetItemsWithOutParameter(out int value)
    {
        value = 42;
        return GetItemsAsync(1);
    }
}
