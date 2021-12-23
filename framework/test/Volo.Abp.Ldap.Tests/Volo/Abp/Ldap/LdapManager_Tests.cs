using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Ldap;

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
    public async Task AuthenticateAsync()
    {
        (await _ldapManager.AuthenticateAsync("cn=abp,dc=abp,dc=io", "123qwe")).ShouldBe(true);
        (await _ldapManager.AuthenticateAsync("cn=abp,dc=abp,dc=io", "123123")).ShouldBe(false);
        (await _ldapManager.AuthenticateAsync("NoExists", "123qwe")).ShouldBe(false);
    }
}
