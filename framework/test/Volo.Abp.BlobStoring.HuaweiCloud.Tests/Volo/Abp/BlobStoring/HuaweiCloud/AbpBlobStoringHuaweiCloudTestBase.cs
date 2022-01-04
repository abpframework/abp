using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.HuaweiCloud;

public class AbpBlobStoringHuaweiCloudTestCommonBase : AbpIntegratedTest<AbpBlobStoringHuaweiCloudTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class AbpBlobStoringHuaweiCloudTestBase : AbpIntegratedTest<AbpBlobStoringHuaweiCloudTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
