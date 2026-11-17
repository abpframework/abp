using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class ConcurrentScopeTestInterceptor : AbpScopeInterceptor
{
    public static readonly AsyncLocal<string?> ScopeId = new();

    protected override IAbpInterceptionScope BeginScope(IAbpMethodInvocation invocation)
    {
        var scopeId = Guid.NewGuid().ToString("N");
        ScopeId.Value = scopeId;
        return new EventRecordingScope((AsyncEnumerableConcurrentScopedTestObject)invocation.TargetObject, scopeId);
    }

    private class EventRecordingScope : IAbpInterceptionScope
    {
        private readonly AsyncEnumerableConcurrentScopedTestObject _target;
        private readonly string _scopeId;

        public EventRecordingScope(AsyncEnumerableConcurrentScopedTestObject target, string scopeId)
        {
            _target = target;
            _scopeId = scopeId;
        }

        public Task CompleteAsync(CancellationToken cancellationToken)
        {
            _target.Events.Enqueue($"{_scopeId}:Completed");
            return Task.CompletedTask;
        }

        public Task FailAsync(Exception exception)
        {
            _target.Events.Enqueue($"{_scopeId}:Failed");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _target.Events.Enqueue($"{_scopeId}:Disposed");
        }
    }
}
