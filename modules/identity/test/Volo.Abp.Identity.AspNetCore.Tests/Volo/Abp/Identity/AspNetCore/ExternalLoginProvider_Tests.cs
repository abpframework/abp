using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore
{
    public class ExternalLoginProvider_Tests : AbpIdentityAspNetCoreTestBase
    {
        [Fact]
        public async Task Should_SignIn_With_ExternalLoginProvider()
        {
            var result = await GetResponseAsStringAsync(
                "api/signin-test/password?userName=ext_user&password=abc"
            );

            result.ShouldBe("Succeeded");
        }
    }
}
