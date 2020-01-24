using Volo.Abp.Testing;

namespace Volo.Abp.Authorization
{
    public class AuthorizationTestBase : AbpIntegratedTest<AbpAuthorizationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}