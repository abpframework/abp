using Volo.Abp.Testing;

namespace Volo.Abp.IdentityServer;

public class AbpIdentityServerTestBase : AbpIntegratedTest<AbpIdentityServerTestEntityFrameworkCoreModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
