using Volo.Abp.Testing;

namespace Volo.Abp.TestApp
{
    public class TestAppTestBase : AbpIntegratedTest<TestAppTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}