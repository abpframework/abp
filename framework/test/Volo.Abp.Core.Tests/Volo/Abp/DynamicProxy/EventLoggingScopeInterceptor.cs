using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class EventLoggingScopeInterceptor : AbpScopeInterceptor
{
    private readonly ScopeEventLog _log;
    private int _scopeCount;

    public EventLoggingScopeInterceptor(ScopeEventLog log)
    {
        _log = log;
    }

    protected override bool ShouldIntercept(IAbpMethodInvocation invocation)
    {
        return invocation.Method.Name != nameof(AsyncEnumerableScopeInterceptorTestObject.GetItemsWithoutScopeAsync) &&
               base.ShouldIntercept(invocation);
    }

    protected override IAbpInterceptionScope BeginScope(IAbpMethodInvocation invocation)
    {
        var name = $"Scope{Interlocked.Increment(ref _scopeCount)}";
        _log.Events.Add($"{name}.Begin");

        // Only the scopes of the enumerations fail, so the call succeeds.
        var isEnumeration = name != "Scope1";
        return new EventLoggingScope(
            _log,
            name,
            failOnComplete: isEnumeration && invocation.Method.Name == nameof(AsyncEnumerableScopeInterceptorTestObject.GetItemsWithFailingCompleteAsync),
            failOnFail: isEnumeration && invocation.Method.Name == nameof(AsyncEnumerableScopeInterceptorTestObject.GetItemsWithFailingFailAsync));
    }

    private class EventLoggingScope : IAbpInterceptionScope
    {
        private readonly ScopeEventLog _log;
        private readonly string _name;
        private readonly bool _failOnComplete;
        private readonly bool _failOnFail;

        public EventLoggingScope(ScopeEventLog log, string name, bool failOnComplete, bool failOnFail)
        {
            _log = log;
            _name = name;
            _failOnComplete = failOnComplete;
            _failOnFail = failOnFail;
        }

        public async Task CompleteAsync(CancellationToken cancellationToken)
        {
            _log.Events.Add($"{_name}.Complete");
            if (_failOnComplete)
            {
                await Task.Yield();
                throw new InvalidOperationException("Complete failed");
            }
        }

        public async Task FailAsync(Exception exception)
        {
            _log.Events.Add($"{_name}.Fail:{exception.Message}");
            if (_failOnFail)
            {
                await Task.Yield();
                throw new InvalidOperationException("Fail failed");
            }
        }

        public void Dispose()
        {
            _log.Events.Add($"{_name}.Dispose");
        }
    }
}
