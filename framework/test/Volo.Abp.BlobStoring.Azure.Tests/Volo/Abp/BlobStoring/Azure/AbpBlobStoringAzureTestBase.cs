using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring.Azure
{
    public class AbpBlobStoringAzureTestBase : AbpIntegratedTest<AbpBlobStoringAzureTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
