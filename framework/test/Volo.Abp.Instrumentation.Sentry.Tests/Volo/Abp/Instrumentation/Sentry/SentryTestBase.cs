using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sentry;
using Volo.Abp.Instrumentation.Sentry.TestServices;
using Volo.Abp.Testing;

namespace Volo.Abp.Instrumentation.Sentry;

public class SentryTestBase : AbpIntegratedTest<AbpSentryTestModule>
{
    protected readonly IHub SentryHub;
    protected readonly ILogger<MyInstrumentedBySentryService1> Logger;
    
    protected SentryTestBase()
    {
        SentryHub = Substitute.For<IHub>();
        Logger = Substitute.For<ILogger<MyInstrumentedBySentryService1>>();
    }
    
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient(typeof(IHub), (_) => SentryHub));
        services.Replace(ServiceDescriptor.Transient(typeof(ILogger<MyInstrumentedBySentryService1>),(_) => Logger));
    }
}