using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Aws;

public class AbpBlobStoringAwsTestCommonBase : AbpIntegratedTest<AbpBlobStoringAwsTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class AbpBlobStoringAwsTestBase : AbpIntegratedTest<AbpBlobStoringAwsTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
