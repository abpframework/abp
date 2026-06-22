using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class LinkUserTokenProvider_Tests : AbpSingleActiveTokenProviderTestBase
{
    protected IdentityLinkUserManager IdentityLinkUserManager { get; }

    public LinkUserTokenProvider_Tests()
    {
        IdentityLinkUserManager = GetRequiredService<IdentityLinkUserManager>();
    }

    protected override Task<string> GenerateTokenAsync(IdentityUser user)
        => IdentityLinkUserManager.GenerateLinkTokenAsync(
            new IdentityLinkUserInfo(user.Id, user.TenantId),
            LinkUserTokenProviderConsts.LinkUserTokenPurpose);

    protected override Task<bool> VerifyTokenAsync(IdentityUser user, string token)
        => IdentityLinkUserManager.VerifyLinkTokenAsync(
            new IdentityLinkUserInfo(user.Id, user.TenantId),
            token,
            LinkUserTokenProviderConsts.LinkUserTokenPurpose);

    protected override string GetProviderName()
        => LinkUserTokenProviderConsts.LinkUserTokenProviderName;

    protected override string GetPurpose()
        => LinkUserTokenProviderConsts.LinkUserTokenPurpose;

    [Fact]
    public void LinkUserTokenProvider_Should_Be_Registered()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContain(x =>
            x.Key == LinkUserTokenProviderConsts.LinkUserTokenProviderName &&
            x.Value.ProviderType == typeof(LinkUserTokenProvider));
    }

    [Theory]
    [InlineData("TestTokenPurpose1")]
    [InlineData("TestTokenPurpose2")]
    [InlineData("TestTokenPurpose3")]
    public virtual async Task GenerateAndVerifyLinkTokenAsync_With_Custom_Purpose(string tokenPurpose)
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var token = await IdentityLinkUserManager.GenerateLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), tokenPurpose);
            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), token, tokenPurpose)).ShouldBeTrue();
            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), "123123", tokenPurpose)).ShouldBeFalse();
            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RemoveLinkUserTokenAsync_Should_Invalidate_Stored_Hash()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var token = await GenerateTokenAsync(john);

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await UserManager.RemoveLinkUserTokenAsync(john)).Succeeded.ShouldBeTrue();

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(john, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RemoveLinkUserTokenAsync_With_Custom_Purpose_Should_Invalidate_Only_That_Purpose()
    {
        const string customPurpose = "CustomLinkUserPurpose";

        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var defaultPurposeToken = await GenerateTokenAsync(john);
            var customPurposeToken = await IdentityLinkUserManager.GenerateLinkTokenAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                customPurpose);

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await UserManager.RemoveLinkUserTokenAsync(john, customPurpose)).Succeeded.ShouldBeTrue();

            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), customPurposeToken, customPurpose)).ShouldBeFalse();
            (await VerifyTokenAsync(john, defaultPurposeToken)).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task GenerateLinkTokenAsync_With_Custom_Purpose_Should_Invalidate_Previous_Token_For_Same_Purpose()
    {
        const string customPurpose = "CustomLinkUserPurpose";

        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var firstToken = await IdentityLinkUserManager.GenerateLinkTokenAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                customPurpose);
            var secondToken = await IdentityLinkUserManager.GenerateLinkTokenAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                customPurpose);

            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), firstToken, customPurpose)).ShouldBeFalse();
            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), secondToken, customPurpose)).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }
}
