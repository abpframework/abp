using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class AbpResourceOwnerPasswordValidator_Tests : AbpResourceOwnerPasswordValidatorTestBase
{
    [Fact]
    public async Task Invalid_Recovery_Code_Should_Not_Be_Overwritten_By_RequiresTwoFactor()
    {
        await CreateUserAsync(twoFactorEnabled: true);
        var context = CreateContext(new NameValueCollection
        {
            ["RecoveryCode"] = "invalid-recovery-code"
        });

        await ValidateAsync(context);

        context.Result.IsError.ShouldBeTrue();
        context.Result.Error.ShouldBe("invalid_grant");
        context.Result.ErrorDescription.ShouldBe("Invalid recovery code!");
    }

    [Fact]
    public async Task Recovery_Code_Should_Remain_Usable_When_Redemption_Update_Fails()
    {
        await CreateUserAsync(twoFactorEnabled: true);
        var recoveryCode = await GenerateRecoveryCodeAsync();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAfterSuccessfulUpdates(0);
        var failedContext = CreateContext(new NameValueCollection
        {
            ["RecoveryCode"] = recoveryCode
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

        var retryContext = CreateContext(new NameValueCollection
        {
            ["RecoveryCode"] = recoveryCode
        });
        await ValidateAsync(retryContext);

        retryContext.Result.IsError.ShouldBeFalse();
        retryContext.Result.Subject.ShouldNotBeNull();
    }

    [Fact]
    public async Task ChangePassword_Update_Exception_Should_Return_InvalidGrant_And_Preserve_User_State()
    {
        await CreateUserAsync(twoFactorEnabled: false, shouldChangePasswordOnNextLogin: true);
        var challengeContext = CreateContext();
        await ValidateAsync(challengeContext);
        var changePasswordToken = challengeContext.Result.CustomResponse["changePasswordToken"].ToString();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAfterSuccessfulUpdates(0);
        var context = CreateContext(new NameValueCollection
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        });
        try
        {
            await ValidateAsync(context);
        }
        finally
        {
            failureSimulator.Reset();
        }

        context.Result.IsError.ShouldBeTrue();
        context.Result.Error.ShouldBe("invalid_grant");

        var userState = await GetPasswordStateAsync();
        userState.OldPasswordIsValid.ShouldBeTrue();
        userState.NewPasswordIsValid.ShouldBeFalse();
        userState.ShouldChangePasswordOnNextLogin.ShouldBeTrue();
    }

    [Fact]
    public async Task Valid_TwoFactor_Code_Should_Reset_Access_Failed_Count()
    {
        await CreateUserAsync(twoFactorEnabled: true, addFailedAccess: true);
        var code = await GenerateTwoFactorCodeAsync();
        var context = CreateContext(new NameValueCollection
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        });

        await ValidateAsync(context);

        context.Result.IsError.ShouldBeFalse();
        context.Result.Subject.ShouldNotBeNull();
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Valid_Recovery_Code_Should_Reset_Access_Failed_Count()
    {
        await CreateUserAsync(twoFactorEnabled: true, addFailedAccess: true);
        var recoveryCode = await GenerateRecoveryCodeAsync();
        var context = CreateContext(new NameValueCollection
        {
            ["RecoveryCode"] = recoveryCode
        });

        await ValidateAsync(context);

        context.Result.IsError.ShouldBeFalse();
        context.Result.Subject.ShouldNotBeNull();
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Reset_Failure_Should_Not_Authenticate()
    {
        await CreateUserAsync(twoFactorEnabled: true, addFailedAccess: true);
        var code = await GenerateTwoFactorCodeAsync();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAccessFailedCountReset();
        var context = CreateContext(new NameValueCollection
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        });
        try
        {
            await ValidateAsync(context);
        }
        finally
        {
            failureSimulator.Reset();
        }

        context.Result.IsError.ShouldBeTrue();
        context.Result.Error.ShouldBe("invalid_grant");
        (await GetAccessFailedCountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Recovery_Code_Should_Remain_Usable_When_Reset_Failure_Rolls_Back_Request()
    {
        await CreateUserAsync(twoFactorEnabled: true, addFailedAccess: true);
        var recoveryCode = await GenerateRecoveryCodeAsync();
        var failureSimulator = GetRequiredService<IdentityUserStoreFailureSimulator>();
        failureSimulator.FailAccessFailedCountReset();
        var failedContext = CreateContext(new NameValueCollection
        {
            ["RecoveryCode"] = recoveryCode
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
        (await GetAccessFailedCountAsync()).ShouldBe(1);

        var retryContext = CreateContext(new NameValueCollection
        {
            ["RecoveryCode"] = recoveryCode
        });
        await ValidateAsync(retryContext);

        retryContext.Result.IsError.ShouldBeFalse();
        retryContext.Result.Subject.ShouldNotBeNull();
        (await GetAccessFailedCountAsync()).ShouldBe(0);
    }
}
