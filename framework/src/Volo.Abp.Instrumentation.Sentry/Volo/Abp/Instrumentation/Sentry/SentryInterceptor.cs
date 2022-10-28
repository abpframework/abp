using Microsoft.Extensions.Logging;
using Sentry;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Instrumentation.Sentry.Volo.Abp.Instrumentation.Sentry;

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
        if (SentryHelper.IsSentrySpanMethod(invocation.Method, out var SentrySpanAttribute))
        {
            var operation = $"{invocation.Method?.DeclaringType?.Name}.{invocation.Method?.Name}";
            var description = "";
            var span = _sentryHub.GetSpan()?.StartChild(operation, description);

            if (_logger.IsEnabled(LogLevel.Debug) && span is not null)
            {
                _logger.LogDebug($"{nameof(SentryInterceptor)} start child span with operation: {operation}, with SpanId: {span.SpanId}, in ParentSpanId: {span.ParentSpanId}");
            } else if (_logger.IsEnabled(LogLevel.Warning) && span is null)
            {
                _logger.LogWarning($"{nameof(SentryInterceptor)} cannot started child with operation: {operation}. Check Sentry configuration.");
            }
            
            await invocation.ProceedAsync();
            
            span?.Finish();
            return;
        }
        
        await invocation.ProceedAsync();
    }
}