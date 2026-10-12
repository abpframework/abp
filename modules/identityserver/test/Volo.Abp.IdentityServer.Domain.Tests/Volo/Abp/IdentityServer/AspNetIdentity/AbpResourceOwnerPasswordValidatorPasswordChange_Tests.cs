using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Identity.Settings;
using Xunit;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class AbpResourceOwnerPasswordValidatorPasswordChange_Tests : AbpResourceOwnerPasswordValidatorTestBase
{
    [Fact]
    public async Task Required_Password_Change_With_TwoFactor_Should_Require_Code_Before_Authenticating()
    {
        await CreateUserAsync(
            twoFactorEnabled: true,
            shouldChangePasswordOnNextLogin: true,
            addFailedAccess: true);

        var challengeContext = CreateContext();
        await ValidateAsync(challengeContext);
        var changePasswordToken = AssertPasswordChangeChallenge(
            challengeContext,
            "ShouldChangePasswordOnNextLogin");

        var passwordChangeContext = CreateContext(new NameValueCollection
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        });
        await ValidateAsync(passwordChangeContext);

        AssertRequiresTwoFactor(passwordChangeContext);
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var code = await GenerateTwoFactorCodeAsync();
        var successContext = CreateContext(new NameValueCollection
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        }, NewPassword);
        await ValidateAsync(successContext);

        successContext.Result.IsError.ShouldBeFalse();
        successContext.Result.Subject.ShouldNotBeNull();
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Required_Password_Change_Without_TwoFactor_Should_Authenticate_And_Reset_Failed_Count()
    {
        await CreateUserAsync(
            twoFactorEnabled: false,
            shouldChangePasswordOnNextLogin: true,
            addFailedAccess: true);

        var challengeContext = CreateContext();
        await ValidateAsync(challengeContext);
        var changePasswordToken = AssertPasswordChangeChallenge(
            challengeContext,
            "ShouldChangePasswordOnNextLogin");

        var successContext = CreateContext(new NameValueCollection
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        });
        await ValidateAsync(successContext);

        successContext.Result.IsError.ShouldBeFalse();
        successContext.Result.Subject.ShouldNotBeNull();
        (await GetAccessFailedCountAsync()).ShouldBe(0);

        var passwordState = await GetPasswordStateAsync();
        passwordState.OldPasswordIsValid.ShouldBeFalse();
        passwordState.NewPasswordIsValid.ShouldBeTrue();
        passwordState.ShouldChangePasswordOnNextLogin.ShouldBeFalse();
    }

    [Fact]
    public async Task Periodic_Password_Change_With_TwoFactor_Should_Use_Periodic_Challenge_And_Require_Code()
    {
        var settingValueProvider = GetRequiredService<IdentityServerTestSettingValueProvider>();
        settingValueProvider.Set(
            IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword,
            true.ToString());
        settingValueProvider.Set(
            IdentitySettingNames.Password.PasswordChangePeriodDays,
            1.ToString());

        try
        {
            await CreateUserAsync(
                twoFactorEnabled: true,
                addFailedAccess: true,
                lastPasswordChangeTime: DateTimeOffset.UtcNow.AddDays(-2));

            var challengeContext = CreateContext();
            await ValidateAsync(challengeContext);
            var changePasswordToken = AssertPasswordChangeChallenge(
                challengeContext,
                "PeriodicallyChangePassword");

            var passwordChangeContext = CreateContext(new NameValueCollection
            {
                ["NewPassword"] = NewPassword,
                ["ChangePasswordToken"] = changePasswordToken
            });
            await ValidateAsync(passwordChangeContext);

            AssertRequiresTwoFactor(passwordChangeContext);
            (await GetAccessFailedCountAsync()).ShouldBe(1);

            var code = await GenerateTwoFactorCodeAsync();
            var successContext = CreateContext(new NameValueCollection
            {
                ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
                ["TwoFactorCode"] = code
            }, NewPassword);
            await ValidateAsync(successContext);

            successContext.Result.IsError.ShouldBeFalse();
            successContext.Result.Subject.ShouldNotBeNull();
            (await GetAccessFailedCountAsync()).ShouldBe(0);
        }
        finally
        {
            settingValueProvider.Clear();
        }
    }

    [Fact]
    public async Task Password_Change_Should_Roll_Back_When_Access_Failed_Count_Update_Fails()
    {
        await CreateUserAsync(
            twoFactorEnabled: true,
            shouldChangePasswordOnNextLogin: true);

        var challengeContext = CreateContext();
        await ValidateAsync(challengeContext);
        var changePasswordToken = AssertPasswordChangeChallenge(
            challengeContext,
            "ShouldChangePasswordOnNextLogin");

        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAfterSuccessfulUpdates(2);
        var failedContext = CreateContext(new NameValueCollection
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken,
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = "invalid-code"
        });
        try
        {
            await ValidateAsync(failedContext);
        }
        finally
        {
            failureSimulator.Reset();
        }

        failedContext.Result.IsError.ShouldBeTrue();
        failedContext.Result.Error.ShouldBe("invalid_grant");
        (await GetAccessFailedCountAsync()).ShouldBe(0);

        var passwordState = await GetPasswordStateAsync();
        passwordState.OldPasswordIsValid.ShouldBeTrue();
        passwordState.NewPasswordIsValid.ShouldBeFalse();
        passwordState.ShouldChangePasswordOnNextLogin.ShouldBeTrue();

        var retryContext = CreateContext();
        await ValidateAsync(retryContext);
        AssertPasswordChangeChallenge(retryContext, "ShouldChangePasswordOnNextLogin");
    }

    private static string AssertPasswordChangeChallenge(
        ResourceOwnerPasswordValidationContext context,
        string expectedErrorDescription)
    {
        context.Result.IsError.ShouldBeTrue();
        context.Result.Error.ShouldBe("invalid_grant");
        context.Result.ErrorDescription.ShouldBe(expectedErrorDescription);
        context.Result.CustomResponse.ShouldNotBeNull();
        context.Result.CustomResponse.ContainsKey("changePasswordToken").ShouldBeTrue();
        return context.Result.CustomResponse["changePasswordToken"].ToString();
    }

    private static void AssertRequiresTwoFactor(ResourceOwnerPasswordValidationContext context)
    {
        context.Result.IsError.ShouldBeTrue();
        context.Result.Error.ShouldBe("invalid_grant");
        context.Result.ErrorDescription.ShouldBe("RequiresTwoFactor");
        context.Result.CustomResponse.ShouldNotBeNull();
        context.Result.CustomResponse.ContainsKey("twoFactorToken").ShouldBeTrue();
    }
}
