using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Instrumentation.Sentry.TestServices;

public interface IMyInstrumentedBySentryService1 : ITransientDependency
{
    public Task ClassHadSentrySpanButMethodNot();
    
    public Task ClassHadSentrySpanButMethodNotWillInvokeNextMethod();

    public Task ClassHadSentrySpanButMethodNotNextMethod();
}