using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Instrumentation.Sentry;

[DependsOn(
    typeof(AbpSentryModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
)]
public class AbpSentryTestModule : AbpModule
{
    
}