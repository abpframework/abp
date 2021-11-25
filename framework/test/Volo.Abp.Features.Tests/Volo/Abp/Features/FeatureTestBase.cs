using Volo.Abp.Testing;

namespace Volo.Abp.Features;

public class FeatureTestBase : AbpIntegratedTest<AbpFeaturesTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
