using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore
{
    public class LinkUserTokenProvider_Tests : AbpIdentityAspNetCoreTestBase
    {
        [Fact]
        public void LinkUserTokenProvider_Should_Be_Register()
        {
            var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

            identityOptions.Tokens.ProviderMap.ShouldContain(x =>
                x.Key == LinkUserTokenProvider.LinkUserTokenProviderName &&
                x.Value.ProviderType == typeof(LinkUserTokenProvider));
        }
    }
}
