using Volo.Abp.Testing;

namespace Volo.Abp.TextTemplating
{
    public abstract class AbpTextTemplatingTestBase : AbpIntegratedTest<AbpTextTemplatingTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
