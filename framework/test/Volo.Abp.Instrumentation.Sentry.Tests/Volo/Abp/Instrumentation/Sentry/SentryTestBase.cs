using Volo.Abp.Testing;

namespace Volo.Abp.Instrumentation.Sentry;

public class SentryTestBase : AbpIntegratedTest<AbpSentryTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}