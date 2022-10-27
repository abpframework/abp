using Sentry;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Instrumentation.Sentry.Volo.Abp.Instrumentation.Sentry;

public class SentryInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IHub _sentryHub;

    public SentryInterceptor(IHub sentryHub)
    {
        _sentryHub = sentryHub;
    }
    
    public override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        throw new NotImplementedException();
    }
}