using Volo.Abp.Testing;

namespace Volo.Abp.GlobalFeatures;

public abstract class GlobalFeatureTestBase : AbpIntegratedTest<GlobalFeatureTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
