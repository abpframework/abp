using Volo.Abp.Testing;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingAspNetCoreTestBase : AbpIntegratedTest<AbpImagingAspNetCoreTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}