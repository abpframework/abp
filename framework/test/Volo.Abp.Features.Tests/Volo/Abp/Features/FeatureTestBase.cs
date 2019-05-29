namespace Volo.Abp.Features
{
    public class FeatureTestBase : AbpIntegratedTest<FeaturesTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
