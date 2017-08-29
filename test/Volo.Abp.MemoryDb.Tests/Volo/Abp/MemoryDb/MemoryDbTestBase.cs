using Volo.Abp.TestBase;

namespace Volo.Abp.MemoryDb.Repositories
{
    public abstract class MemoryDbTestBase : AbpIntegratedTest<AbpMemoryDbTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
