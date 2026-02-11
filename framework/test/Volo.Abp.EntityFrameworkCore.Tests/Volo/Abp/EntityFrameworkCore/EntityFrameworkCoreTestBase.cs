using Volo.Abp.TestApp.Testing;

namespace Volo.Abp.EntityFrameworkCore;

public abstract class EntityFrameworkCoreTestBase : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
