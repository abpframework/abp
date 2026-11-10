using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class AbpResourceOwnerPasswordValidatorExternalLogin_Tests : AbpResourceOwnerPasswordValidatorTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpIdentityOptions>(options =>
        {
            options.ExternalLoginProviders.Add<TestExternalLoginProvider>(TestExternalLoginProvider.Name);
            options.ExternalLoginProviders.Add<DisabledTestExternalLoginProvider>(DisabledTestExternalLoginProvider.Name);
        });
    }

    [Fact]
    public async Task External_Login_Should_Authenticate()
    {
        await CreateUserAsync(twoFactorEnabled: false);
        var context = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        context.Result.IsError.ShouldBeFalse();
        context.Result.Subject.ShouldNotBeNull();
    }

    [Fact]
    public async Task External_Login_With_TwoFactor_Should_Require_Code_Before_Authenticating()
    {
        await CreateUserAsync(twoFactorEnabled: true);
        var challengeContext = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(challengeContext);

        AssertInvalidGrant(challengeContext, "RequiresTwoFactor");

        var code = await GenerateTwoFactorCodeAsync();
        var context = CreateContext(new NameValueCollection
        {
            ["TwoFactorProvider"] = TokenOptions.DefaultEmailProvider,
            ["TwoFactorCode"] = code
        }, TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        context.Result.IsError.ShouldBeFalse();
        context.Result.Subject.ShouldNotBeNull();
    }

    [Fact]
    public async Task External_Login_Should_Not_Authenticate_Inactive_User()
    {
        await CreateUserAsync(twoFactorEnabled: false);
        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetIsActive(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });
        var context = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        AssertInvalidGrant(
            context,
            "You are not allowed to login! Your account is inactive or needs to confirm your email/phone number.");
    }

    [Fact]
    public async Task External_Login_Should_Not_Authenticate_Locked_Out_User()
    {
        await CreateUserAsync(twoFactorEnabled: false);
        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });
        var context = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        AssertInvalidGrant(
            context,
            "The user account has been locked out due to invalid login attempts. Please wait a while and try again.");
    }

    [Fact]
    public async Task External_Login_Should_Not_Authenticate_User_That_Requires_Confirmation()
    {
        GetRequiredService<IdentityServerTestSettingValueProvider>().Set(
            IdentitySettingNames.SignIn.RequireConfirmedEmail,
            true.ToString());

        await CreateUserAsync(twoFactorEnabled: false);
        await UpdateUserAsync(async (userManager, user) =>
        {
            user.SetEmailConfirmed(false);
            (await userManager.UpdateAsync(user)).CheckErrors();
        });
        var context = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        AssertInvalidGrant(
            context,
            "You are not allowed to login! Your account is inactive or needs to confirm your email/phone number.");
    }

    [Fact]
    public async Task External_Login_Should_Not_Authenticate_User_That_Should_Change_Password()
    {
        await CreateUserAsync(twoFactorEnabled: false, shouldChangePasswordOnNextLogin: true);
        var context = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        AssertInvalidGrant(context, "ExternalLoginPasswordChangeRequired");
    }

    [Fact]
    public async Task External_Login_Should_Not_Change_Password_Of_User_That_Should_Change_Password()
    {
        await CreateUserAsync(twoFactorEnabled: false, shouldChangePasswordOnNextLogin: true);
        string changePasswordToken = null;
        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.RemovePasswordAsync(user)).CheckErrors();
            (await userManager.AddPasswordAsync(user, TestExternalLoginProvider.Password)).CheckErrors();
            changePasswordToken = await userManager.GenerateUserTokenAsync(
                user,
                TokenOptions.DefaultProvider,
                "ShouldChangePasswordOnNextLogin");
        });
        var context = CreateContext(new NameValueCollection
        {
            ["NewPassword"] = NewPassword,
            ["ChangePasswordToken"] = changePasswordToken
        }, TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        AssertInvalidGrant(context, "ExternalLoginPasswordChangeRequired");

        await UpdateUserAsync(async (userManager, user) =>
        {
            (await userManager.CheckPasswordAsync(user, TestExternalLoginProvider.Password)).ShouldBeTrue();
            (await userManager.CheckPasswordAsync(user, NewPassword)).ShouldBeFalse();
            user.ShouldChangePasswordOnNextLogin.ShouldBeTrue();
        });
    }

    [Fact]
    public async Task External_Login_Should_Not_Authenticate_User_That_Should_Periodically_Change_Password()
    {
        var settingValueProvider = GetRequiredService<IdentityServerTestSettingValueProvider>();
        settingValueProvider.Set(
            IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword,
            true.ToString());
        settingValueProvider.Set(
            IdentitySettingNames.Password.PasswordChangePeriodDays,
            1.ToString());

        await CreateUserAsync(
            twoFactorEnabled: false,
            lastPasswordChangeTime: DateTimeOffset.UtcNow.AddDays(-2));
        var context = CreateContext(password: TestExternalLoginProvider.Password);

        await ValidateAsync(context);

        AssertInvalidGrant(context, "ExternalLoginPasswordChangeRequired");
    }

    [Fact]
    public async Task Disabled_External_Login_Provider_Should_Not_Be_Used()
    {
        await CreateUserAsync(twoFactorEnabled: false);
        var context = CreateContext(password: "neither-local-nor-external-1q2w3E*");

        await ValidateAsync(context);

        AssertInvalidGrant(context, "Invalid username or password!");
    }

    private Task UpdateUserAsync(Func<IdentityUserManager, IdentityUser, Task> action)
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(UserName);
            await action(userManager, user);
        });
    }

    private static void AssertInvalidGrant(
        ResourceOwnerPasswordValidationContext context,
        string expectedErrorDescription)
    {
        context.Result.IsError.ShouldBeTrue();
        context.Result.Error.ShouldBe("invalid_grant");
        context.Result.ErrorDescription.ShouldBe(expectedErrorDescription);
        context.Result.Subject.ShouldBeNull();
        (context.Result.CustomResponse?.ContainsKey("changePasswordToken") ?? false).ShouldBeFalse();
    }
}
