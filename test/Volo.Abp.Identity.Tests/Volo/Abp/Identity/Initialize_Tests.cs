using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Identity
{
    public class Initialize_Tests : AbpIntegratedTest<AbpIdentityTestModule>
    {
        [Fact]
        public void Should_Initialize_Identity_Module()
        {
            var userManager = ServiceProvider.GetRequiredService<IdentityUserManager>();
        }
    }
}
