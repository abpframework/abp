using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Autofac.Interception;

public class Autofac_Interception_Test : AbpInterceptionTestBase<AutofacTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
