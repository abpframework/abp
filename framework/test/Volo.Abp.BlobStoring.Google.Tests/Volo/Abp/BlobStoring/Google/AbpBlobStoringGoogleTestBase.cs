using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Google;

public class AbpBlobStoringGoogleTestCommonBase : AbpIntegratedTest<AbpBlobStoringGoogleTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class AbpBlobStoringGoogleTestBase : AbpIntegratedTest<AbpBlobStoringGoogleTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
