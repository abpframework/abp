using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpDefaultTokenProvider_Tests : AbpSingleActiveTokenProviderTestBase
{
    private const string TestPurpose = nameof(SignInResult.RequiresTwoFactor);

    // Matches ChangePasswordType.ShouldChangePasswordOnNextLogin.ToString() in
    // AbpResourceOwnerPasswordValidator (Volo.Abp.IdentityServer.Domain). Hard-coded
    // here to avoid taking a project dependency on the IdentityServer module.
    private const string ChangePasswordPurpose = "ShouldChangePasswordOnNextLogin";

    protected override Task<string> GenerateTokenAsync(IdentityUser user)
        => UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose);

    protected override Task<bool> VerifyTokenAsync(IdentityUser user, string token)
        => UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose, token);

    protected override string GetProviderName()
        => TokenOptions.DefaultProvider;

    protected override string GetPurpose()
        => TestPurpose;

    [Fact]
    public void AbpDefaultTokenProvider_Should_Override_AspNetCore_Default()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(TokenOptions.DefaultProvider);
        identityOptions.Tokens.ProviderMap[TokenOptions.DefaultProvider].ProviderType
            .ShouldBe(typeof(AbpDefaultTokenProvider));
    }

    [Fact]
    public void AbpDefaultTokenProviderOptions_Should_Default_To_TenMinutes()
    {
        var options = GetRequiredService<IOptions<AbpDefaultTokenProviderOptions>>().Value;

        options.Name.ShouldBe(TokenOptions.DefaultProvider);
        options.TokenLifespan.ShouldBe(TimeSpan.FromMinutes(10));
    }

    [Fact]
    public async Task Tokens_For_Different_Purposes_Should_Be_Independent()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            var twoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose);
            var changePasswordToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, ChangePasswordPurpose);

            user = await UserRepository.GetAsync(TestData.UserJohnId);

            (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose, twoFactorToken)).ShouldBeTrue();
            (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, ChangePasswordPurpose, changePasswordToken)).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Regenerating_Same_Purpose_Should_Invalidate_Only_That_Purpose()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            var firstTwoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose);
            var changePasswordToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, ChangePasswordPurpose);

            user = await UserRepository.GetAsync(TestData.UserJohnId);
            var secondTwoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose);

            user = await UserRepository.GetAsync(TestData.UserJohnId);

            (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose, firstTwoFactorToken)).ShouldBeFalse();
            (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, TestPurpose, secondTwoFactorToken)).ShouldBeTrue();
            (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, ChangePasswordPurpose, changePasswordToken)).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Token_Should_Be_Invalid_After_SecurityStamp_Change()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            var token = await GenerateTokenAsync(user);

            user = await UserRepository.GetAsync(TestData.UserJohnId);
            (await UserManager.UpdateSecurityStampAsync(user)).Succeeded.ShouldBeTrue();

            user = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(user, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Token_Should_Not_Verify_Against_Different_User()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var johnToken = await GenerateTokenAsync(john);

            var david = await UserRepository.GetAsync(TestData.UserDavidId);
            (await VerifyTokenAsync(david, johnToken)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }
}
