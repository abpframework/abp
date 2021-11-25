using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class LinkUserTokenProvider_Tests : AbpIdentityAspNetCoreTestBase
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }
    protected IdentityLinkUserManager IdentityLinkUserManager { get; }
    protected IdentityTestData TestData { get; }

    public LinkUserTokenProvider_Tests()
    {
        UserRepository = GetRequiredService<IIdentityUserRepository>();
        IdentityLinkUserRepository = GetRequiredService<IIdentityLinkUserRepository>();
        IdentityLinkUserManager = GetRequiredService<IdentityLinkUserManager>();
        TestData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public void LinkUserTokenProvider_Should_Be_Register()
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
    public virtual async Task GenerateAndVerifyLinkTokenAsync(string tokenPurpose)
    {
        var john = await UserRepository.GetAsync(TestData.UserJohnId);
        var token = await IdentityLinkUserManager.GenerateLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), tokenPurpose);
        (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), token, tokenPurpose)).ShouldBeTrue();
        (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), "123123", tokenPurpose)).ShouldBeFalse();
    }
}
