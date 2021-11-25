using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class ExternalLoginProvider_Tests : AbpIdentityAspNetCoreTestBase
{
    [Fact]
    public async Task Should_SignIn_With_ExternalLoginProvider()
    {
        // User does not exists yet
        (await GetRequiredService<IdentityUserManager>().FindByNameAsync("ext_user")).ShouldBeNull();

        // Try to login

        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_user&password=abc"
        );

        result.ShouldBe("Succeeded");

        // User should be created now

        await CheckUserAsync();

        // Re-login

        result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_user&password=abc"
        );

        result.ShouldBe("Succeeded");

        await CheckUserAsync();
    }

    private async Task CheckUserAsync()
    {
        var userRepository = GetRequiredService<IIdentityUserRepository>();

        var user = await userRepository.FindByNormalizedUserNameAsync("EXT_USER");
        user.Name.ShouldBe("Test Name");
        user.Surname.ShouldBe("Test Surname");
        user.EmailConfirmed.ShouldBeTrue();
        user.PhoneNumber.ShouldBe("123");
        user.PhoneNumberConfirmed.ShouldBeFalse();
        user.IsExternal.ShouldBeTrue();

        var logins = user.Logins.Where(l => l.LoginProvider == "Fake").ToList();
        logins.Count.ShouldBe(1);
        logins[0].ProviderKey.ShouldBe("123");
    }
}
