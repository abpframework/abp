using Volo.Abp.Testing;

namespace Volo.Abp.Account
{
    public class AbpAccountApplicationTestBase : AbpIntegratedTest<AbpAccountApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}