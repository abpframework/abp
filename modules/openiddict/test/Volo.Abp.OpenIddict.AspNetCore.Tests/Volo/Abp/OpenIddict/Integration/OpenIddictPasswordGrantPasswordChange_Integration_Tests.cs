using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.OpenIddict.Integration;

public class OpenIddictPasswordGrantPasswordChange_Integration_Tests : OpenIddictPasswordGrantIntegrationTestBase
{
    [Fact]
    public async Task Required_Password_Change_With_TwoFactor_Should_Require_Code_Before_Issuing_Token()
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: true,
            shouldChangePasswordOnNextLogin: true,
            addFailedAccess: true);

        var challenge = await RequestPasswordTokenAsync();
        var changePasswordToken = await AssertPasswordChangeChallengeAsync(
            challenge,
            "ShouldChangePasswordOnNextLogin");

        var passwordChangeResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        });

        await AssertRequiresTwoFactorAsync(passwordChangeResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var code = await GenerateTwoFactorCodeAsync();
        var successResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        }, NewPassword);

        await AssertAccessTokenAsync(successResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Required_Password_Change_Without_TwoFactor_Should_Issue_Token_And_Reset_Failed_Count()
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: false,
            shouldChangePasswordOnNextLogin: true,
            addFailedAccess: true);

        var challenge = await RequestPasswordTokenAsync();
        var changePasswordToken = await AssertPasswordChangeChallengeAsync(
            challenge,
            "ShouldChangePasswordOnNextLogin");

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        });

        await AssertAccessTokenAsync(response);
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Periodic_Password_Change_With_TwoFactor_Should_Require_Code_Before_Issuing_Token()
    {
        var settingValueProvider = GetRequiredService<OpenIddictTestSettingValueProvider>();
        settingValueProvider.Set(
            IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword,
            true.ToString());
        settingValueProvider.Set(
            IdentitySettingNames.Password.PasswordChangePeriodDays,
            1.ToString());

        try
        {
            await ConfigurePasswordChangeUserAsync(
                twoFactorEnabled: true,
                addFailedAccess: true,
                lastPasswordChangeTime: DateTimeOffset.UtcNow.AddDays(-2));

            var challenge = await RequestPasswordTokenAsync();
            var changePasswordToken = await AssertPasswordChangeChallengeAsync(
                challenge,
                "PeriodicallyChangePassword");

            var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["NewPassword"] = NewPassword,
                ["ChangePasswordToken"] = changePasswordToken
            });

            await AssertRequiresTwoFactorAsync(response);
            (await GetAccessFailedCountAsync()).ShouldBe(1);

            var code = await GenerateTwoFactorCodeAsync();
            var successResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
                ["TwoFactorCode"] = code
            }, NewPassword);

            await AssertAccessTokenAsync(successResponse);
            (await GetAccessFailedCountAsync()).ShouldBe(0);
        }
        finally
        {
            settingValueProvider.Clear();
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Password_Change_Update_Failure_Should_Roll_Back_Password_And_Required_Change_State(
        int successfulUpdatesBeforeFailure)
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: false,
            shouldChangePasswordOnNextLogin: true);

        var challenge = await RequestPasswordTokenAsync();
        var changePasswordToken = await AssertPasswordChangeChallengeAsync(
            challenge,
            "ShouldChangePasswordOnNextLogin");

        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAfterSuccessfulUpdates(successfulUpdatesBeforeFailure);
        HttpResponseMessage failedResponse;
        try
        {
            failedResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["NewPassword"] = NewPassword,
                ["ChangePasswordToken"] = changePasswordToken
            });
        }
        finally
        {
            failureSimulator.Reset();
        }

        await AssertInvalidGrantAsync(failedResponse);

        var passwordState = await GetPasswordStateAsync();
        passwordState.OldPasswordIsValid.ShouldBeTrue();
        passwordState.NewPasswordIsValid.ShouldBeFalse();
        passwordState.ShouldChangePasswordOnNextLogin.ShouldBeTrue();

        var retryChallenge = await RequestPasswordTokenAsync();
        await AssertPasswordChangeChallengeAsync(
            retryChallenge,
            "ShouldChangePasswordOnNextLogin");
    }

    [Fact]
    public async Task Password_Change_Should_Roll_Back_When_Access_Failed_Count_Update_Fails()
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: true,
            shouldChangePasswordOnNextLogin: true);

        var challenge = await RequestPasswordTokenAsync();
        var changePasswordToken = await AssertPasswordChangeChallengeAsync(
            challenge,
            "ShouldChangePasswordOnNextLogin");

        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAfterSuccessfulUpdates(2);
        HttpResponseMessage failedResponse;
        try
        {
            failedResponse = await RequestPasswordTokenAsync(new Dictionary<string, string>
            {
                ["NewPassword"] = NewPassword,
                ["ChangePasswordToken"] = changePasswordToken,
                ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
                ["TwoFactorCode"] = "invalid-code"
            });
        }
        finally
        {
            failureSimulator.Reset();
        }

        await AssertInvalidGrantAsync(failedResponse);
        (await GetAccessFailedCountAsync()).ShouldBe(0);

        var passwordState = await GetPasswordStateAsync();
        passwordState.OldPasswordIsValid.ShouldBeTrue();
        passwordState.NewPasswordIsValid.ShouldBeFalse();
        passwordState.ShouldChangePasswordOnNextLogin.ShouldBeTrue();

        var retryChallenge = await RequestPasswordTokenAsync();
        await AssertPasswordChangeChallengeAsync(
            retryChallenge,
            "ShouldChangePasswordOnNextLogin");
    }

    [Fact]
    public async Task Required_Password_Change_Should_Not_Be_Offered_To_Inactive_User()
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: false,
            shouldChangePasswordOnNextLogin: true);
        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetIsActive(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync();

        await AssertNotAllowedAsync(response, AbpOpenIddictErrors.AccountInactive);
    }

    [Fact]
    public async Task Required_Password_Change_Should_Not_Be_Completed_By_Inactive_User()
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: false,
            shouldChangePasswordOnNextLogin: true);
        string changePasswordToken = null;
        await UpdateUserAsync(async (userManager, user) =>
        {
            changePasswordToken = await userManager.GenerateUserTokenAsync(
                user,
                TokenOptions.DefaultProvider,
                "ShouldChangePasswordOnNextLogin");
            user.SetIsActive(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync(new Dictionary<string, string>
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        });

        await AssertNotAllowedAsync(response, AbpOpenIddictErrors.AccountInactive);

        var passwordState = await GetPasswordStateAsync();
        passwordState.OldPasswordIsValid.ShouldBeTrue();
        passwordState.NewPasswordIsValid.ShouldBeFalse();
        passwordState.ShouldChangePasswordOnNextLogin.ShouldBeTrue();
    }

    [Fact]
    public async Task Required_Password_Change_Should_Not_Be_Offered_To_Locked_Out_User()
    {
        await ConfigurePasswordChangeUserAsync(
            twoFactorEnabled: false,
            shouldChangePasswordOnNextLogin: true);
        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync();

        await AssertNotAllowedAsync(response, AbpOpenIddictErrors.AccountLocked);
    }

    [Fact]
    public async Task Required_Password_Change_Should_Not_Be_Offered_To_User_That_Requires_Confirmation()
    {
        var settingValueProvider = GetRequiredService<OpenIddictTestSettingValueProvider>();
        settingValueProvider.Set(IdentitySettingNames.SignIn.RequireConfirmedEmail, true.ToString());

        try
        {
            await ConfigurePasswordChangeUserAsync(
                twoFactorEnabled: false,
                shouldChangePasswordOnNextLogin: true);
            await UpdateUserAsync(async (userManager, user) =>
            {
                user.SetEmailConfirmed(false);
                (await userManager.UpdateAsync(user)).CheckErrors();
            });

            var response = await RequestPasswordTokenAsync();

            await AssertNotAllowedAsync(response, "invalid_grant", AbpErrorDescriptionConsts.RequiresConfirmUser);
        }
        finally
        {
            settingValueProvider.Clear();
        }
    }

    private Task UpdateUserAsync(Func<IdentityUserManager, Volo.Abp.Identity.IdentityUser, Task> action)
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            await action(userManager, user);
        });
    }

    private static async Task AssertNotAllowedAsync(
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

    private static async Task<string> AssertPasswordChangeChallengeAsync(
        HttpResponseMessage response,
        string expectedErrorDescription)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        root.GetProperty("error_description").GetString().ShouldBe(expectedErrorDescription);
        root.TryGetProperty("access_token", out _).ShouldBeFalse();
        return root.GetProperty("changePasswordToken").GetString();
    }

    private static async Task AssertRequiresTwoFactorAsync(HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        root.GetProperty("error_description").GetString().ShouldBe("RequiresTwoFactor");
        root.TryGetProperty("access_token", out _).ShouldBeFalse();
    }

    private Task ConfigurePasswordChangeUserAsync(
        bool twoFactorEnabled,
        bool shouldChangePasswordOnNextLogin = false,
        bool addFailedAccess = false,
        DateTimeOffset? lastPasswordChangeTime = null)
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            user.SetShouldChangePasswordOnNextLogin(shouldChangePasswordOnNextLogin);
            if (lastPasswordChangeTime.HasValue)
            {
                user.SetLastPasswordChangeTime(lastPasswordChangeTime);
            }

            (await userManager.SetTwoFactorEnabledAsync(user, twoFactorEnabled)).CheckErrors();
            if (addFailedAccess)
            {
                (await userManager.AccessFailedAsync(user)).CheckErrors();
            }
        });
    }

    private Task<(bool OldPasswordIsValid, bool NewPasswordIsValid, bool ShouldChangePasswordOnNextLogin)> GetPasswordStateAsync()
    {
        return WithUnitOfWorkAsync(
            new AbpUnitOfWorkOptions { IsTransactional = false },
            async serviceProvider =>
            {
                var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
                var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
                return (
                    OldPasswordIsValid: await userManager.CheckPasswordAsync(user, OpenIddictPasswordGrantTestData.Password),
                    NewPasswordIsValid: await userManager.CheckPasswordAsync(user, NewPassword),
                    user.ShouldChangePasswordOnNextLogin);
            });
    }
}
