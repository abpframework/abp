using Volo.Abp.Testing;

namespace Volo.Abp.BlobStoring
{
    public abstract class AbpBlobStoringTestBase : AbpIntegratedTest<AbpBlobStoringTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}