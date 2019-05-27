namespace Volo.Abp.IdentityServer
{
    public class AbpIdentityServerDomainTestBase : AbpIntegratedTest<IdentityServerDomainTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
