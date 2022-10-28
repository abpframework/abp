using Volo.Abp.Autofac;
using Volo.Abp.Instrumentation.Sentry;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.Auditing;
public class AbpAuditingTestBase : AbpIntegratedTest<AbpAuditingTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
