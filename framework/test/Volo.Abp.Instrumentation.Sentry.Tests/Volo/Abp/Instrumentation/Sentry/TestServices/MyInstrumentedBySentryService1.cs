using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Instrumentation.Sentry.TestServices;

[SentrySpan]
public class MyInstrumentedBySentryService1 : IMyInstrumentedBySentryService1, IApplicationService
{
    private readonly ILogger<MyInstrumentedBySentryService1> _logger;

    public MyInstrumentedBySentryService1(ILogger<MyInstrumentedBySentryService1> logger)
    {
        _logger = logger;
    }
    
    public virtual async Task ClassHadSentrySpanButMethodNot()
    {
        await Task.Delay(1);
        _logger.LogInformation($"Invoke {nameof(ClassHadSentrySpanButMethodNot)}");
    }

    public virtual async Task ClassHadSentrySpanButMethodNotWillInvokeNextMethod()
    {
        await Task.Delay(1);
        _logger.LogInformation($"Invoke {nameof(ClassHadSentrySpanButMethodNotWillInvokeNextMethod)}");
        await this.ClassHadSentrySpanButMethodNotNextMethod();
    }

    public virtual async Task ClassHadSentrySpanButMethodNotNextMethod()
    {
        await Task.Delay(1);
        _logger.LogInformation($"Invoke {nameof(ClassHadSentrySpanButMethodNotNextMethod)}");
    }
}