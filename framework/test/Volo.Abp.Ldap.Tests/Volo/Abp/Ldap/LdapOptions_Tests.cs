using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Ldap
{
    public class LdapOptions_Tests : AbpIntegratedTest<AbpLdapTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        [Fact]
        public void Should_Resolve_AbpAbpLdapOptionsFactory()
        {
            GetRequiredService<IOptions<AbpLdapOptions>>().ShouldBeOfType(typeof(AbpAbpLdapOptionsManager));
        }
    }
}
