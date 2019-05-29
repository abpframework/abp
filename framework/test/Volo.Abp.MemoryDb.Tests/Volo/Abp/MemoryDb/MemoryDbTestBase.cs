namespace Volo.Abp.MemoryDb
{
    public abstract class MemoryDbTestBase : AbpIntegratedTest<MemoryDbTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
