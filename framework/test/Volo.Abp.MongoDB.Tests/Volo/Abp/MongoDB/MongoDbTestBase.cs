namespace Volo.Abp.MongoDB
{
    public abstract class MongoDbTestBase : AbpIntegratedTest<MongoDbTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}