using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.Application;

public abstract class AbpDddApplicationTestBase : AbpIntegratedTest<AbpDddApplicationTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
