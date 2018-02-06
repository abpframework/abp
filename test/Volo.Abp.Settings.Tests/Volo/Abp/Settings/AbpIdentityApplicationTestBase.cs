using Volo.Abp.TestBase;

namespace Volo.Abp.Settings
{
    public class AbpSettingsTestBase : AbpIntegratedTest<AbpSettingsTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
