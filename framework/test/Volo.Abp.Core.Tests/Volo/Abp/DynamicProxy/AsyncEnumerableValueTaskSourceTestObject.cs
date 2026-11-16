using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Volo.Abp.DynamicProxy;

public class AsyncEnumerableValueTaskSourceTestObject : AsyncEnumerableScopedTestObject
{
    public virtual IAsyncEnumerable<int> GetItemsAsync(bool completeAsynchronously)
    {
        return new ValueTaskSourceSequence(Logs, completeAsynchronously, completeWithoutExecutionContext: false);
    }

    public virtual IAsyncEnumerable<int> GetItemsCompletedWithoutExecutionContextAsync()
    {
        return new ValueTaskSourceSequence(Logs, completeAsynchronously: true, completeWithoutExecutionContext: true);
    }

    /// <summary>
    /// Records the scope value when its value tasks are completed and their results are read.
    /// </summary>
    private class ValueTaskSourceSequence : IAsyncEnumerable<int>, IAsyncEnumerator<int>, IValueTaskSource<bool>, IValueTaskSource
    {
        private readonly List<string> _logs;
        private readonly bool _completeAsynchronously;
        private readonly bool _completeWithoutExecutionContext;
        private ManualResetValueTaskSourceCore<bool> _core;
        private bool _result;
        private int _current;

        public ValueTaskSourceSequence(List<string> logs, bool completeAsynchronously, bool completeWithoutExecutionContext)
        {
            _logs = logs;
            _completeAsynchronously = completeAsynchronously;
            _completeWithoutExecutionContext = completeWithoutExecutionContext;
        }

        public int Current => _current;

        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            _core.Reset();
            Complete(++_current <= 2);
            return new ValueTask<bool>(this, _core.Version);
        }

        public ValueTask DisposeAsync()
        {
            _core.Reset();
            Complete(false);
            return new ValueTask(this, _core.Version);
        }

        private void Complete(bool result)
        {
            if (_completeWithoutExecutionContext)
            {
                // Completed when the continuation is registered, so it is always registered first.
                _result = result;
                return;
            }

            if (!_completeAsynchronously)
            {
                _core.SetResult(result);
                return;
            }

            Task.Run(async () =>
            {
                await Task.Delay(10);
                _core.SetResult(result);
            });
        }

        bool IValueTaskSource<bool>.GetResult(short token)
        {
            _logs.Add($"MoveNext:{ScopeTestInterceptor.ScopeValue.Value}");
            return _core.GetResult(token);
        }

        ValueTaskSourceStatus IValueTaskSource<bool>.GetStatus(short token) => _core.GetStatus(token);

        void IValueTaskSource<bool>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            OnCompleted(continuation, state, token, flags);
        }

        void IValueTaskSource.GetResult(short token)
        {
            _logs.Add($"Dispose:{ScopeTestInterceptor.ScopeValue.Value}");
            _core.GetResult(token);
        }

        ValueTaskSourceStatus IValueTaskSource.GetStatus(short token) => _core.GetStatus(token);

        void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            OnCompleted(continuation, state, token, flags);
        }

        private void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            _core.OnCompleted(continuation, state, token, flags);

            if (!_completeWithoutExecutionContext)
            {
                return;
            }

            _logs.Add($"OnCompleted:{ScopeTestInterceptor.ScopeValue.Value}");

            // Like an I/O completion, the value task is completed on a thread without the execution context of the awaiter.
            using (ExecutionContext.SuppressFlow())
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    _logs.Add($"SetResult:{ScopeTestInterceptor.ScopeValue.Value}");
                    _core.SetResult(_result);
                });
            }
        }
    }
}
