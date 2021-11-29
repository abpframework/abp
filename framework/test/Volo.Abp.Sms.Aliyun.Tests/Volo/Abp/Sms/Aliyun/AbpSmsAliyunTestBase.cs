using Volo.Abp.Testing;

namespace Volo.Abp.Sms.Aliyun
{
    public class AbpSmsAliyunTestBase : AbpIntegratedTest<AbpSmsAliyunTestsModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}