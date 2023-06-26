using Volo.Abp.Testing;

namespace Volo.Abp.Imaging;

public abstract class AbpImagingAbstractionsTestBase : AbpIntegratedTest<AbpImagingAbstractionsTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}