using Volo.Abp.Testing;

namespace Volo.Abp.Auditing;

public class AbpAuditingTestBase : AbpIntegratedTest<AbpAuditingTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
