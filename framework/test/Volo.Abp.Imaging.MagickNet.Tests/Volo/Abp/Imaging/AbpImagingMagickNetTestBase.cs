using Volo.Abp.Testing;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingMagickNetTestBase : AbpIntegratedTest<AbpImagingMagickNetTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}