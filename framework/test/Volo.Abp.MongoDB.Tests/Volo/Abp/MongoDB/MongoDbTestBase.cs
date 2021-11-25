using Volo.Abp.Testing;

namespace Volo.Abp.MongoDB;

public abstract class MongoDbTestBase : AbpIntegratedTest<AbpMongoDbTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
