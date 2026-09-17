using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Bunny;

public class AbpBlobStoringBunnyTestCommonBase : AbpIntegratedTest<AbpBlobStoringBunnyTestCommonModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class AbpBlobStoringBunnyTestBase : AbpIntegratedTest<AbpBlobStoringBunnyTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
