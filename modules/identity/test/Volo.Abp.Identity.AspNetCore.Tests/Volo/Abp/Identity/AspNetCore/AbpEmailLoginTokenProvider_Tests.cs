using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpEmailLoginTokenProvider_Tests : AbpSingleActiveTokenProviderTestBase
{
    protected override Task<string> GenerateTokenAsync(IdentityUser user)
        => UserManager.GenerateEmailLoginTokenAsync(user);

    protected override Task<bool> VerifyTokenAsync(IdentityUser user, string token)
        => UserManager.VerifyEmailLoginTokenAsync(user, token);

    protected override string GetProviderName()
        => AbpEmailLoginTokenProvider.ProviderName;

    protected override string GetPurpose()
        => AbpEmailLoginTokenPurposes.EmailLogin;

    [Fact]
    public void AbpEmailLoginTokenProvider_Should_Be_Registered()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(AbpEmailLoginTokenProvider.ProviderName);
        identityOptions.Tokens.ProviderMap[AbpEmailLoginTokenProvider.ProviderName].ProviderType
            .ShouldBe(typeof(AbpEmailLoginTokenProvider));
    }

    [Fact]
    public async Task RemoveEmailLoginTokenAsync_Should_Invalidate_Token()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);

            var token = await UserManager.GenerateEmailLoginTokenAsync(john);
            (await UserManager.VerifyEmailLoginTokenAsync(john, token)).ShouldBeTrue();

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            await UserManager.RemoveEmailLoginTokenAsync(john);

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await UserManager.VerifyEmailLoginTokenAsync(john, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }
}
