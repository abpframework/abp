using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpSignInManager_Tests : AbpIdentityAspNetCoreTestBase
{
    [Fact]
    public void Should_Resolve_AbpSignInManager()
    {
        var signInManager = GetRequiredService<SignInManager<IdentityUser>>();
        signInManager.ShouldBeOfType<AbpSignInManager>();
    }

    [Fact]
    public async Task Should_SignIn_With_Correct_Credentials()
    {
        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=admin&password=1q2w3E*"
        );

        result.ShouldBe("Succeeded");
    }

    [Fact]
    public async Task Should_Not_SignIn_With_Wrong_Credentials()
    {
        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=admin&password=WRONG_PASSWORD"
        );

        result.ShouldBe("Failed");
    }

    [Fact]
    public async Task Should_Not_SignIn_If_User_Not_Active()
    {
        var result = await GetResponseAsStringAsync(
            "api/signin-test/password?userName=bob&password=1q2w3E*"
        );

        result.ShouldBe("NotAllowed");
    }

    [Fact]
    public async Task Should_Return_NotAllowed_For_User_That_Should_Change_Password_Regardless_Of_Password()
    {
        // PreSignInCheck rejects users with ShouldChangePasswordOnNextLogin=true before the
        // password is verified, so PasswordSignInAsync returns NotAllowed for both right and
        // wrong passwords. Callers that surface a login error to the user (Login page) need
        // to recheck the password themselves to distinguish the two cases.
        var userManager = GetRequiredService<IdentityUserManager>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        const string userName = "must-change-password";
        const string password = "1q2w3E*";

        using (var uow = unitOfWorkManager.Begin())
        {
            var user = new IdentityUser(Guid.NewGuid(), userName, userName + "@abp.io");
            user.SetShouldChangePasswordOnNextLogin(true);
            (await userManager.CreateAsync(user, password)).Succeeded.ShouldBeTrue();
            await uow.CompleteAsync();
        }

        var withWrongPassword = await GetResponseAsStringAsync(
            $"api/signin-test/password?userName={userName}&password=WRONG_PASSWORD"
        );
        withWrongPassword.ShouldBe("NotAllowed");

        var withCorrectPassword = await GetResponseAsStringAsync(
            $"api/signin-test/password?userName={userName}&password={password}"
        );
        withCorrectPassword.ShouldBe("NotAllowed");
    }
}
