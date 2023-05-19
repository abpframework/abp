using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Minio;

public class AbpBlobStoringMinioTestCommonBase : AbpIntegratedTest<AbpBlobStoringMinioTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class AbpBlobStoringMinioTestBase : AbpIntegratedTest<AbpBlobStoringMinioTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
