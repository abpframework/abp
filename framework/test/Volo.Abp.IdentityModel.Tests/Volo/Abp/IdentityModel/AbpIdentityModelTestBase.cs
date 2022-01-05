using Volo.Abp.Testing;

namespace Volo.Abp.IdentityModel;

public abstract class AbpIdentityModelTestBase : AbpIntegratedTest<AbpIdentityModelTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
