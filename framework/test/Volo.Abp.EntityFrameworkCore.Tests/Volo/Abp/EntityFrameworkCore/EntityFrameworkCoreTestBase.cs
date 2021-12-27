using Volo.Abp.Testing;

namespace Volo.Abp.EntityFrameworkCore;

public abstract class EntityFrameworkCoreTestBase : AbpIntegratedTest<AbpEntityFrameworkCoreTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
