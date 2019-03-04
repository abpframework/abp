namespace Volo.Abp.Features
{
    public class AbpFeaturesTestBase : AbpIntegratedTest<AbpFeaturesTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
