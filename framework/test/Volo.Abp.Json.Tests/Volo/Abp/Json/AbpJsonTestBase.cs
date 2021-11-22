using Volo.Abp.Testing;

namespace Volo.Abp.Json;

public abstract class AbpJsonTestBase : AbpIntegratedTest<AbpJsonTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
