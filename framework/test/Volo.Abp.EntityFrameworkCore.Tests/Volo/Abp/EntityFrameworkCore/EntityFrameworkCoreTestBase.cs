namespace Volo.Abp.EntityFrameworkCore
{
    public abstract class EntityFrameworkCoreTestBase : AbpIntegratedTest<EntityFrameworkCoreTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
