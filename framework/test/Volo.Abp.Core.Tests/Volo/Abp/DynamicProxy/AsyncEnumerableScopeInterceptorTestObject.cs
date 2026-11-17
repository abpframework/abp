using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableScopeInterceptorTestObject
{
    private readonly ScopeEventLog _log;

    public AsyncEnumerableScopeInterceptorTestObject(ScopeEventLog log)
    {
        _log = log;
    }

    public virtual Task<int> GetValueAsync()
    {
        Interlocked.Increment(ref _log.TargetInvocations);
        return Task.FromResult(42);
    }

    public virtual async IAsyncEnumerable<int> GetItemsWithoutScopeAsync()
    {
        await Task.Yield();
        yield return 1;
    }

    public virtual IAsyncEnumerable<int> GetItemsAsync(bool throwOnCall = false, bool throwOnDispose = false)
    {
        Interlocked.Increment(ref _log.TargetInvocations);
        if (throwOnCall)
        {
            throw new InvalidOperationException("Failed on call");
        }

        return ProduceAsync(throwOnDispose);
    }

    public virtual IAsyncEnumerable<int> GetItemsThrowingOnGetAsyncEnumerator()
    {
        Interlocked.Increment(ref _log.TargetInvocations);
        return new ThrowingOnGetAsyncEnumeratorSequence();
    }

    public virtual IAsyncEnumerable<int> GetItemsThrowingOnCurrent(bool throwOnDispose = false)
    {
        Interlocked.Increment(ref _log.TargetInvocations);
        return new ThrowingOnCurrentSequence(_log, throwOnDispose);
    }

    public virtual async IAsyncEnumerable<int> GetItemsWithFailingCompleteAsync()
    {
        await Task.Yield();
        yield return 1;
    }

    public virtual async IAsyncEnumerable<int> GetItemsWithFailingFailAsync()
    {
        await Task.Yield();
        yield return 1;
        throw new InvalidOperationException("Source failed");
    }

    private class ThrowingOnCurrentSequence : IAsyncEnumerable<int>, IAsyncEnumerator<int>
    {
        private readonly ScopeEventLog _log;
        private readonly bool _throwOnDispose;

        public ThrowingOnCurrentSequence(ScopeEventLog log, bool throwOnDispose)
        {
            _log = log;
            _throwOnDispose = throwOnDispose;
        }

        public int Current => throw new InvalidOperationException("Failed on Current");

        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(true);
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Yield();
            _log.Events.Add("Source.Dispose");
            if (_throwOnDispose)
            {
                throw new InvalidOperationException("Failed on dispose");
            }
        }
    }

    private class ThrowingOnGetAsyncEnumeratorSequence : IAsyncEnumerable<int>
    {
        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Failed on GetAsyncEnumerator");
        }
    }

    private static async IAsyncEnumerable<int> ProduceAsync(bool throwOnDispose)
    {
        try
        {
            await Task.Yield();
            yield return 1;
        }
        finally
        {
            if (throwOnDispose)
            {
                throw new InvalidOperationException("Failed on dispose");
            }
        }
    }
}
