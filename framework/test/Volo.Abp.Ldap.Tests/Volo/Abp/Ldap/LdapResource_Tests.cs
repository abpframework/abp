using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Ldap.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Ldap;

public class LdapResource_Tests : AbpIntegratedTest<AbpLdapTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public void LdapResource_Test()
    {
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<LdapResource>>()["DisplayName:Abp.Ldap.ServerHost"].Value.ShouldBe("Server host");
        }

        using (CultureHelper.Use("tr"))
        {
            GetRequiredService<IStringLocalizer<LdapResource>>()["DisplayName:Abp.Ldap.ServerHost"].Value.ShouldBe("Sunucu Ana Bilgisayarı");
        }
    }
}
