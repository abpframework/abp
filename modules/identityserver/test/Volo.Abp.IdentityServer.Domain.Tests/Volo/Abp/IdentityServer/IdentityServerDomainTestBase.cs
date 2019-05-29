namespace Volo.Abp.IdentityServer
{
    public class IdentityServerDomainTestBase : AbpIntegratedTest<IdentityServerDomainTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
