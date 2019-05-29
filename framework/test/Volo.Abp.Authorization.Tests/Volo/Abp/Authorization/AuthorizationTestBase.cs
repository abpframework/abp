namespace Volo.Abp.Authorization
{
    public class AuthorizationTestBase : AbpIntegratedTest<AuthorizationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}