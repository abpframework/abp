using Volo.Abp.Testing;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public abstract class AbpAspNetCoreMvcClientTestBase : AbpIntegratedTest<AbpAspNetCoreMvcClientTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
