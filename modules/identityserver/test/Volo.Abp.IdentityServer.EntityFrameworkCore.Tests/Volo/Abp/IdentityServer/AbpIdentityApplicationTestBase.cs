namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerTestBase : AbpIntegratedTest<IdentityServerTestEntityFrameworkCoreModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
