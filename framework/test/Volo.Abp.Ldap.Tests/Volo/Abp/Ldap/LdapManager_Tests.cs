using Shouldly;
using Volo.Abp.Testing;
using Xunit;

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

        [Fact(Skip = "Required Ldap environment")]
        public void Authenticate()
        {
            _ldapManager.Authenticate("cn=abp,dc=abp,dc=io", "123qwe").ShouldBe(true);
            _ldapManager.Authenticate("NoExists", "123qwe").ShouldBe(false);
        }
    }
}
