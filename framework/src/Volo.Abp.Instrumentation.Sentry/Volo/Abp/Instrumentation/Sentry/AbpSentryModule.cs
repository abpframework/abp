using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Instrumentation.Sentry.Volo.Abp.Instrumentation.Sentry;

public class AbpSentryModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(SentryInterceptorRegistrar.RegisterIfNeeded);
    }
}