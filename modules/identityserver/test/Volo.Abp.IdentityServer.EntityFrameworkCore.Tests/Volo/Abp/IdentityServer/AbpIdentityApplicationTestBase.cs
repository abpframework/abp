namespace Volo.Abp.IdentityServer
{
    public class IdentityServerTestBase : AbpIntegratedTest<IdentityServerTestEntityFrameworkCoreModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
