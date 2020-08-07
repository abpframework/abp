using Volo.Abp.Testing;

namespace Volo.Abp.Ldap
{
    public class LdapManager_Tests : AbpIntegratedTest<AbpLdapTestModule>
    {
        private readonly ILdapManager _ldapManager;

        public LdapManager_Tests()
        {
            _ldapManager = GetRequiredService<ILdapManager>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
