using Microsoft.Extensions.Logging;
using Sentry;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Instrumentation.Sentry;

public class SentryInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IHub _sentryHub;
    private readonly ILogger<SentryInterceptor> _logger;

    public SentryInterceptor(IHub sentryHub, ILogger<SentryInterceptor> logger)
    {
        _sentryHub = sentryHub;
        _logger = logger;
    }
    
    public async override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        if (SentryHelper.IsSentrySpanMethod(invocation.Method, out var sentrySpanAttribute))
        {
            var operation = sentrySpanAttribute?.Operation ?? $"{invocation.Method?.DeclaringType?.Name}.{invocation.Method?.Name}";
            var description = sentrySpanAttribute?.Description ?? null;
            var span = _sentryHub.GetSpan()?.StartChild(operation, description);

            if (_logger.IsEnabled(LogLevel.Debug) && span is not null)
            {
                _logger.LogDebug($"{nameof(SentryInterceptor)} start child span with operation: {operation}, with SpanId: {span.SpanId.ToString()}, in ParentSpanId: {span.ParentSpanId.ToString()}");
            }

            if (span is null && sentrySpanAttribute is SentryTransactionAttribute sentryTransactionAttribute)
            {
                span = _sentryHub.StartTransaction(sentryTransactionAttribute.Name, operation, description);
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        $"{nameof(SentryInterceptor)} start transaction with name: {sentryTransactionAttribute.Name}, with operation: {operation}, with TransactionId: {span.SpanId.ToString()}");
                }
            }

            if (_logger.IsEnabled(LogLevel.Warning) && span is null)
            {
                _logger.LogWarning($"{nameof(SentryInterceptor)} cannot start child span. Method: {invocation.Method?.DeclaringType?.Name}.{invocation.Method?.Name} should had {nameof(sentryTransactionAttribute)}.");
            }

            await invocation.ProceedAsync();
            
            span?.Finish();
            return;
        }
        
        await invocation.ProceedAsync();
    }
}