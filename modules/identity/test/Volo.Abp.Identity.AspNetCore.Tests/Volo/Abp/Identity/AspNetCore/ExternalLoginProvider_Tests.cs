using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Uow;
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

    [Fact]
    public async Task Should_Not_SignIn_With_ExternalLoginProvider_If_User_Not_Active()
    {
        await CreateExternalUserAsync(async (userManager, user) =>
        {
            user.SetIsActive(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_user&password=abc"
        );

        result.ShouldBe("NotAllowed");
    }

    [Fact]
    public async Task Should_Not_SignIn_With_ExternalLoginProvider_If_User_Should_Change_Password()
    {
        await CreateExternalUserAsync(async (userManager, user) =>
        {
            user.SetShouldChangePasswordOnNextLogin(true);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_user&password=abc"
        );

        result.ShouldBe("ExternalLoginPasswordChangeRequired");
    }

    [Fact]
    public async Task Should_Not_SignIn_With_ExternalLoginProvider_If_User_Locked_Out()
    {
        await CreateExternalUserAsync(async (userManager, user) =>
        {
            (await userManager.SetLockoutEnabledAsync(user, true)).CheckErrors();
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });

        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_user&password=abc"
        );

        result.ShouldBe("LockedOut");
    }

    [Fact]
    public async Task Should_Not_SignIn_With_ExternalLoginProvider_If_Locked_Out_User_Should_Change_Password()
    {
        await CreateExternalUserAsync(async (userManager, user) =>
        {
            user.SetShouldChangePasswordOnNextLogin(true);
            (await userManager.SetLockoutEnabledAsync(user, true)).CheckErrors();
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });

        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_user&password=abc"
        );

        result.ShouldBe("LockedOut");
    }

    [Fact]
    public async Task Should_Require_TwoFactor_With_ExternalLoginProvider_If_User_TwoFactor_Enabled()
    {
        await CreateExternalUserAsync(async (userManager, user) =>
        {
            (await userManager.SetTwoFactorEnabledAsync(user, true)).CheckErrors();
        }, "ext_2fa_user");

        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=ext_2fa_user&password=abc"
        );

        result.ShouldBe("RequiresTwoFactor");
    }

    [Fact]
    public async Task Should_Not_SignIn_With_Disabled_ExternalLoginProvider()
    {
        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=disabled_ext_user&password=abc"
        );

        result.ShouldBe("Failed");

        (await GetRequiredService<IdentityUserManager>().FindByNameAsync("disabled_ext_user")).ShouldBeNull();
    }

    private async Task CreateExternalUserAsync(
        Func<IdentityUserManager, IdentityUser, Task> configureUser,
        string userName = "ext_user")
    {
        var result = await GetResponseAsStringAsync(
            $"api/signin-test/password?userName={userName}&password=abc"
        );

        result.ShouldBe("Succeeded");

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var userManager = GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(userName);
            await configureUser(userManager, user);
            await uow.CompleteAsync();
        }
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
