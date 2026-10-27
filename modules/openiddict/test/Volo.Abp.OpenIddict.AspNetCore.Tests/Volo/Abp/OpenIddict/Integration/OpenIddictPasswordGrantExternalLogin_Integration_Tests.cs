using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.OpenIddict.Integration;

public class OpenIddictPasswordGrantExternalLogin_Integration_Tests : OpenIddictPasswordGrantIntegrationTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpIdentityOptions>(options =>
        {
            options.ExternalLoginProviders.Add<TestExternalLoginProvider>(TestExternalLoginProvider.Name);
            options.ExternalLoginProviders.Add<DisabledTestExternalLoginProvider>(DisabledTestExternalLoginProvider.Name);
        });
    }

    [Fact]
    public async Task External_Login_Should_Issue_Token()
    {
        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.SetTwoFactorEnabledAsync(user, false)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertAccessTokenAsync(response);
    }

    [Fact]
    public async Task External_Login_With_TwoFactor_Should_Require_Code_Before_Issuing_Token()
    {
        var challenge = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertErrorAsync(challenge, "invalid_grant", AbpErrorDescriptionConsts.RequiresTwoFactor);

        var code = await GenerateTwoFactorCodeAsync();
        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        }, TestExternalLoginProvider.Password);

        await AssertAccessTokenAsync(response);
    }

    [Fact]
    public async Task External_Login_With_Invalid_TwoFactor_Code_Should_Not_Issue_Token()
    {
        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = "invalid-code"
        }, TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, "invalid_grant", "Invalid authenticator code!");
    }

    [Fact]
    public async Task External_Login_With_Valid_Recovery_Code_Should_Issue_Token()
    {
        var recoveryCode = await WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            return (await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1)).Single();
        });

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["RecoveryCode"] = recoveryCode
        }, TestExternalLoginProvider.Password);

        await AssertAccessTokenAsync(response);
    }

    [Fact]
    public async Task External_Login_With_Invalid_Recovery_Code_Should_Not_Issue_Token()
    {
        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["RecoveryCode"] = "invalid-recovery-code"
        }, TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, "invalid_grant", "Invalid recovery code!");
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_Inactive_User()
    {
        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetIsActive(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, AbpOpenIddictErrors.AccountInactive);
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_Inactive_User_With_Valid_TwoFactor_Code()
    {
        var code = await GenerateTwoFactorCodeAsync();
        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetIsActive(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        }, TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, AbpOpenIddictErrors.AccountInactive);
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_Locked_Out_User()
    {
        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, AbpOpenIddictErrors.AccountLocked);
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_Locked_Out_User_With_Valid_TwoFactor_Code()
    {
        var code = await GenerateTwoFactorCodeAsync();
        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        }, TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, AbpOpenIddictErrors.AccountLocked);
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_User_That_Should_Change_Password()
    {
        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetShouldChangePasswordOnNextLogin(true);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, "invalid_grant", "ExternalLoginPasswordChangeRequired");
    }

    [Fact]
    public async Task External_Login_Should_Not_Change_Password_Of_User_That_Should_Change_Password()
    {
        var changePasswordToken = await WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            user.SetShouldChangePasswordOnNextLogin(true);
            (await userManager.RemovePasswordAsync(user)).CheckErrors();
            (await userManager.AddPasswordAsync(user, TestExternalLoginProvider.Password)).CheckErrors();
            return await userManager.GenerateUserTokenAsync(
                user,
                TokenOptions.DefaultProvider,
                "ShouldChangePasswordOnNextLogin");
        });

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        }, TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, "invalid_grant", "ExternalLoginPasswordChangeRequired");

        await WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            (await userManager.CheckPasswordAsync(user, TestExternalLoginProvider.Password)).ShouldBeTrue();
            (await userManager.CheckPasswordAsync(user, NewPassword)).ShouldBeFalse();
            user.ShouldChangePasswordOnNextLogin.ShouldBeTrue();
        });
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_User_That_Should_Periodically_Change_Password()
    {
        var settingValueProvider = GetRequiredService<OpenIddictTestSettingValueProvider>();
        settingValueProvider.Set(
            IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword,
            true.ToString());
        settingValueProvider.Set(
            IdentitySettingNames.Password.PasswordChangePeriodDays,
            1.ToString());

        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetLastPasswordChangeTime(DateTimeOffset.UtcNow.AddDays(-2));
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, "invalid_grant", "ExternalLoginPasswordChangeRequired");
    }

    [Fact]
    public async Task External_Login_Should_Not_Issue_Token_For_User_That_Requires_Confirmation()
    {
        GetRequiredService<OpenIddictTestSettingValueProvider>().Set(
            IdentitySettingNames.SignIn.RequireConfirmedEmail,
            true.ToString());

        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetEmailConfirmed(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(password: TestExternalLoginProvider.Password);

        await AssertErrorAsync(response, "invalid_grant", AbpErrorDescriptionConsts.RequiresConfirmUser);
    }

    [Fact]
    public async Task Disabled_External_Login_Provider_Should_Not_Be_Used()
    {
        var response = await RequestPasswordTokenAsync(password: "neither-local-nor-external-1q2w3E*");

        await AssertErrorAsync(response, "invalid_grant", "Invalid username or password!");
    }

    private Task UpdateUserAsync(Func<IdentityUserManager, IdentityUser, Task> action)
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            await action(userManager, user);
        });
    }

    private static async Task AssertErrorAsync(
        HttpResponseMessage response,
        string expectedError,
        string expectedErrorDescription = null)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        root.GetProperty("error").GetString().ShouldBe(expectedError);
        if (expectedErrorDescription != null)
        {
            root.GetProperty("error_description").GetString().ShouldBe(expectedErrorDescription);
        }

        root.TryGetProperty("access_token", out _).ShouldBeFalse();
        root.TryGetProperty("changePasswordToken", out _).ShouldBeFalse();
    }
}
