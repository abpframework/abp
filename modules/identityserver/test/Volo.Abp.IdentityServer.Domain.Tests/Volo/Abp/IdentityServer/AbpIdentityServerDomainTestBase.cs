using Volo.Abp.Testing;

namespace Volo.Abp.IdentityServer;

public class AbpIdentityServerDomainTestBase : AbpIntegratedTest<AbpIdentityServerDomainTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
