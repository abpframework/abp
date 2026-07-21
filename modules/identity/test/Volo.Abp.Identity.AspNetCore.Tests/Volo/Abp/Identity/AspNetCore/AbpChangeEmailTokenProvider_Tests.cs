using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpChangeEmailTokenProvider_Tests : AbpSingleActiveTokenProviderTestBase
{
    private const string NewEmail = "newemail@example.com";

    protected override Task<string> GenerateTokenAsync(IdentityUser user)
        => UserManager.GenerateChangeEmailTokenAsync(user, NewEmail);

    protected override Task<bool> VerifyTokenAsync(IdentityUser user, string token)
        => UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.ChangeEmailTokenProvider,
            UserManager<IdentityUser>.GetChangeEmailTokenPurpose(NewEmail),
            token);

    protected override string GetProviderName()
        => UserManager.Options.Tokens.ChangeEmailTokenProvider;

    protected override string GetPurpose()
        => UserManager<IdentityUser>.GetChangeEmailTokenPurpose(NewEmail);

    [Fact]
    public void AbpChangeEmailTokenProvider_Should_Be_Registered()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(AbpChangeEmailTokenProvider.ProviderName);
        identityOptions.Tokens.ProviderMap[AbpChangeEmailTokenProvider.ProviderName].ProviderType
            .ShouldBe(typeof(AbpChangeEmailTokenProvider));
    }

    [Fact]
    public void ChangeEmailTokenProvider_Should_Be_Configured_As_Abp()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ChangeEmailTokenProvider.ShouldBe(AbpChangeEmailTokenProvider.ProviderName);
    }

    [Fact]
    public async Task Token_Should_Become_Invalid_After_Email_Change()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);

            var token = await UserManager.GenerateChangeEmailTokenAsync(john, NewEmail);

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            var result = await UserManager.ChangeEmailAsync(john, NewEmail, token);
            result.Succeeded.ShouldBeTrue();

            // SecurityStamp has changed after the email change, so the old token must be invalid.
            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(john, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }
}
