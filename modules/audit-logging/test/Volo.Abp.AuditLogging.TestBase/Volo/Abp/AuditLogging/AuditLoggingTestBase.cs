using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.AuditLogging;

public class AuditLoggingTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
