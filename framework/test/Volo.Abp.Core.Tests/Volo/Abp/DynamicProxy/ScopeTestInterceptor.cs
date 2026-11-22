using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class ScopeTestInterceptor : AbpInterceptor
{
    public static readonly AsyncLocal<string?> ScopeValue = new();

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await invocation.ProceedAsync();

        var logs = ((AsyncEnumerableScopedTestObject)invocation.TargetObject).Logs;
        invocation.WrapAsyncEnumerable(() =>
        {
            ScopeValue.Value = "in-scope";
            return new LoggingScope(logs);
        });
    }

    private class LoggingScope : IAbpInterceptionScope
    {
        private readonly List<string> _logs;

        public LoggingScope(List<string> logs)
        {
            _logs = logs;
        }

        public Task CompleteAsync(CancellationToken cancellationToken)
        {
            _logs.Add($"Completed:{ScopeValue.Value}");
            return Task.CompletedTask;
        }

        public Task FailAsync(Exception exception)
        {
            _logs.Add($"Failed:{exception.Message}");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logs.Add("Disposed");
        }
    }
}
