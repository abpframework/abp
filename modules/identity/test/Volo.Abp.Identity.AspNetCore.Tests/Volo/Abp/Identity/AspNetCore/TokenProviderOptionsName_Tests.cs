using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// The options <c>Name</c> is the Data Protection purpose and the stored-token key prefix, so changing
/// it silently invalidates every token issued before the change, on every host at once. Pinned here.
/// </summary>
public class TokenProviderOptionsName_Tests
{
    [Fact]
    public void Provider_Names_Should_Not_Change()
    {
        new AbpDefaultTokenProviderOptions().Name.ShouldBe(TokenOptions.DefaultProvider);
        new AbpPasswordResetTokenProviderOptions().Name.ShouldBe(AbpPasswordResetTokenProvider.ProviderName);
        new AbpEmailConfirmationTokenProviderOptions().Name.ShouldBe(AbpEmailConfirmationTokenProvider.ProviderName);
        new AbpChangeEmailTokenProviderOptions().Name.ShouldBe(AbpChangeEmailTokenProvider.ProviderName);
        new AbpLinkUserTokenProviderOptions().Name.ShouldBe(LinkUserTokenProviderConsts.LinkUserTokenProviderName);
    }

    [Fact]
    public void Provider_Name_Constants_Should_Not_Change()
    {
        AbpPasswordResetTokenProvider.ProviderName.ShouldBe("AbpPasswordReset");
        AbpEmailConfirmationTokenProvider.ProviderName.ShouldBe("AbpEmailConfirmation");
        AbpChangeEmailTokenProvider.ProviderName.ShouldBe("AbpChangeEmail");
        AbpSingleActiveTokenProvider.InternalLoginProvider.ShouldBe("[AbpSingleActiveToken]");
        LinkUserTokenProviderConsts.LinkUserTokenProviderName.ShouldBe("AbpLinkUser");
    }
}
