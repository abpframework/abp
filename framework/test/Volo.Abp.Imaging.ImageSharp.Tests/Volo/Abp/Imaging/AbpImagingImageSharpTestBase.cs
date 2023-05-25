using Volo.Abp.Testing;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingImageSharpTestBase : AbpIntegratedTest<AbpImagingImageSharpTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}