using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public class AbpBlobStoringHuaweiyunTestCommonBase : AbpIntegratedTest<AbpBlobStoringHuaweiyunTestCommonModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
    public class AbpBlobStoringHuaweiyunTestBase : AbpIntegratedTest<AbpBlobStoringHuaweiyunTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
