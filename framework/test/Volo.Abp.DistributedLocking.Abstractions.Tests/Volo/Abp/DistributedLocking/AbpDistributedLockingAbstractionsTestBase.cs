using Volo.Abp.Testing;

namespace Volo.Abp.DistributedLocking;

public class AbpDistributedLockingAbstractionsTestBase : AbpIntegratedTest<AbpDistributedLockingAbstractionsTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
