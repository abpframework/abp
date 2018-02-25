using Volo.Abp.TestBase;

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