using Volo.Abp.Testing;

namespace Volo.Abp.ExceptionHandling;

public class AbpExceptionHandlingTestBase : AbpIntegratedTest<AbpExceptionHandlingTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
