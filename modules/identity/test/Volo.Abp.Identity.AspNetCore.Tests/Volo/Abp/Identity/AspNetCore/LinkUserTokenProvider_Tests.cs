using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore
{
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

        [Fact]
        public virtual async Task GenerateAndVerifyLinkTokenAsync()
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var token = await IdentityLinkUserManager.GenerateLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId));
            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), token)).ShouldBeTrue();

            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), "123123")).ShouldBeFalse();
        }
    }
}
