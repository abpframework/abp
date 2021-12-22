using Volo.Abp.Testing;

namespace Volo.Abp.MemoryDb;

public abstract class MemoryDbTestBase : AbpIntegratedTest<AbpMemoryDbTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
