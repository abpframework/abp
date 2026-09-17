using Volo.Abp.Testing;

namespace Volo.Abp.Sms.TencentCloud;

public class AbpSmsTencentCloudTestBase : AbpIntegratedTest<AbpSmsTencentCloudTestsModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
