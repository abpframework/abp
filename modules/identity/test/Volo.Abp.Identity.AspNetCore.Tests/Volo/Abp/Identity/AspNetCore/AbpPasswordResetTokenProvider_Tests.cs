using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpPasswordResetTokenProvider_Tests : AbpSingleActiveTokenProviderTestBase
{
    protected override Task<string> GenerateTokenAsync(IdentityUser user)
        => UserManager.GeneratePasswordResetTokenAsync(user);

    protected override Task<bool> VerifyTokenAsync(IdentityUser user, string token)
        => UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<IdentityUser>.ResetPasswordTokenPurpose,
            token);

    protected override string GetProviderName()
        => UserManager.Options.Tokens.PasswordResetTokenProvider;

    protected override string GetPurpose()
        => UserManager<IdentityUser>.ResetPasswordTokenPurpose;

    [Fact]
    public void AbpPasswordResetTokenProvider_Should_Be_Registered()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(AbpPasswordResetTokenProvider.ProviderName);
        identityOptions.Tokens.ProviderMap[AbpPasswordResetTokenProvider.ProviderName].ProviderType
            .ShouldBe(typeof(AbpPasswordResetTokenProvider));
    }

    [Fact]
    public void PasswordResetTokenProvider_Should_Be_Configured_As_Abp()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.PasswordResetTokenProvider.ShouldBe(AbpPasswordResetTokenProvider.ProviderName);
    }

    [Fact]
    public async Task Token_Should_Become_Invalid_After_Password_Reset()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);

            var token = await UserManager.GeneratePasswordResetTokenAsync(john);

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            var result = await UserManager.ResetPasswordAsync(john, token, "1q2w3E*NewP@ss!");
            result.Succeeded.ShouldBeTrue();

            // SecurityStamp has changed after reset, so the old token must be invalid.
            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(john, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }
}
