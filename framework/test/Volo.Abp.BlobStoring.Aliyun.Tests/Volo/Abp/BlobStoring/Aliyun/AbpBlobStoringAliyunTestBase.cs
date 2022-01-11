using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Aliyun;

public class AbpBlobStoringAliyunTestCommonBase : AbpIntegratedTest<AbpBlobStoringAliyunTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
public class AbpBlobStoringAliyunTestBase : AbpIntegratedTest<AbpBlobStoringAliyunTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
