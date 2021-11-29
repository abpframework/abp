using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Azure;

public class AbpBlobStoringAzureTestCommonBase : AbpIntegratedTest<AbpBlobStoringAzureTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class AbpBlobStoringAzureTestBase : AbpIntegratedTest<AbpBlobStoringAzureTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
