using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpEmailConfirmationTokenProvider_Tests : AbpSingleActiveTokenProviderTestBase
{
    protected override Task<string> GenerateTokenAsync(IdentityUser user)
        => UserManager.GenerateEmailConfirmationTokenAsync(user);

    protected override Task<bool> VerifyTokenAsync(IdentityUser user, string token)
        => UserManager.VerifyUserTokenAsync(
            user,
            UserManager.Options.Tokens.EmailConfirmationTokenProvider,
            UserManager<IdentityUser>.ConfirmEmailTokenPurpose,
            token);

    protected override string GetProviderName()
        => UserManager.Options.Tokens.EmailConfirmationTokenProvider;

    protected override string GetPurpose()
        => UserManager<IdentityUser>.ConfirmEmailTokenPurpose;

    [Fact]
    public void AbpEmailConfirmationTokenProvider_Should_Be_Registered()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(AbpEmailConfirmationTokenProvider.ProviderName);
        identityOptions.Tokens.ProviderMap[AbpEmailConfirmationTokenProvider.ProviderName].ProviderType
            .ShouldBe(typeof(AbpEmailConfirmationTokenProvider));
    }

    [Fact]
    public void EmailConfirmationTokenProvider_Should_Be_Configured_As_Abp()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.EmailConfirmationTokenProvider.ShouldBe(AbpEmailConfirmationTokenProvider.ProviderName);
    }

    [Fact]
    public async Task Token_Should_Become_Invalid_After_Email_Confirmation_With_Explicit_Revocation()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);

            var token = await UserManager.GenerateEmailConfirmationTokenAsync(john);

            var result = await UserManager.ConfirmEmailAsync(john, token);
            result.Succeeded.ShouldBeTrue();

            // ConfirmEmailAsync does NOT update SecurityStamp, so the hash is not
            // automatically invalidated. Callers must explicitly revoke the hash.
            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await UserManager.RemoveEmailConfirmationTokenAsync(john)).Succeeded.ShouldBeTrue();

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(john, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }
}
