using Volo.Abp.Testing;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingSkiaSharpTestBase : AbpIntegratedTest<AbpImagingSkiaSharpTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
