using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpEmailTwoFactorTokenProvider_Tests : AbpTwoFactorTokenProviderTestBase
{
    protected override string GetTokenProviderName() => TokenOptions.DefaultEmailProvider;

    protected override string GetInternalProviderName() => AbpEmailTwoFactorTokenProvider.ProviderName;

    [Fact]
    public void AbpEmailTwoFactorTokenProvider_Should_Replace_Default_Email_Provider()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(TokenOptions.DefaultEmailProvider);
        identityOptions.Tokens.ProviderMap[TokenOptions.DefaultEmailProvider].ProviderType
            .ShouldBe(typeof(AbpEmailTwoFactorTokenProvider));
    }

    [Fact]
    public void Default_Options_Should_Match_Documented_Defaults()
    {
        var options = GetRequiredService<IOptions<AbpEmailTwoFactorTokenProviderOptions>>().Value;

        options.CodeLength.ShouldBe(6);
        options.TokenLifespan.ShouldBe(TimeSpan.FromMinutes(3));
    }

    [Fact]
    public async Task CanGenerateTwoFactorTokenAsync_Should_Require_Confirmed_Email()
    {
        using var uow = UnitOfWorkManager.Begin();

        var provider = GetRequiredService<AbpEmailTwoFactorTokenProvider>();
        var john = await UserRepository.GetAsync(TestData.UserJohnId);

        // john.EmailConfirmed defaults to false in the test seed.
        (await provider.CanGenerateTwoFactorTokenAsync(UserManager, john)).ShouldBeFalse();

        john.SetEmailConfirmed(true);
        (await UserManager.UpdateAsync(john)).Succeeded.ShouldBeTrue();

        john = await UserRepository.GetAsync(TestData.UserJohnId);
        (await provider.CanGenerateTwoFactorTokenAsync(UserManager, john)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Email_And_Phone_Tokens_For_Same_User_Should_Be_Independent()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var emailCode = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultEmailProvider, TwoFactorPurpose);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var phoneCode = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, TwoFactorPurpose);

        // Consuming the email code must not invalidate the phone code.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, TwoFactorPurpose, emailCode))
            .ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, TwoFactorPurpose, phoneCode))
            .ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Email_Code_Should_Survive_Security_Stamp_Change()
    {
        // Unlike the default DataProtector-backed token providers, this 2FA provider does
        // not bind the code to the user's SecurityStamp, so rotating the stamp (e.g. a
        // concurrent password change) must NOT invalidate an outstanding 2FA code.
        // This keeps the login flow resilient to legitimate state changes happening
        // between the credential step and the verification step.
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.UpdateSecurityStampAsync(user)).Succeeded.ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Custom_CodeLength_Should_Produce_Code_Of_That_Length()
    {
        using var uow = UnitOfWorkManager.Begin();

        var customProvider = new AbpEmailTwoFactorTokenProvider(
            Microsoft.Extensions.Options.Options.Create(new AbpEmailTwoFactorTokenProviderOptions { CodeLength = 8 }),
            UserRepository,
            GetRequiredService<ICancellationTokenProvider>(),
            Clock,
            GetRequiredService<IDataProtectionProvider>());

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await customProvider.GenerateAsync(TwoFactorPurpose, UserManager, user);

        code.Length.ShouldBe(8);
        code.ShouldMatch(@"^\d{8}$");

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await customProvider.ValidateAsync(TwoFactorPurpose, code, UserManager, user)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Custom_TokenLifespan_Should_Be_Honored()
    {
        using var uow = UnitOfWorkManager.Begin();

        var customLifespan = TimeSpan.FromMinutes(30);

        var customProvider = new AbpEmailTwoFactorTokenProvider(
            Microsoft.Extensions.Options.Options.Create(new AbpEmailTwoFactorTokenProviderOptions { TokenLifespan = customLifespan }),
            UserRepository,
            GetRequiredService<ICancellationTokenProvider>(),
            Clock,
            GetRequiredService<IDataProtectionProvider>());

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var beforeGenerate = new DateTimeOffset(Clock.Now.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(Clock.Now, DateTimeKind.Local)
            : Clock.Now).ToUnixTimeSeconds();
        await customProvider.GenerateAsync(TwoFactorPurpose, UserManager, user);
        var afterGenerate = new DateTimeOffset(Clock.Now.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(Clock.Now, DateTimeKind.Local)
            : Clock.Now).ToUnixTimeSeconds();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var stored = await UserManager.GetAuthenticationTokenAsync(
            user,
            AbpTwoFactorTokenProvider.InternalLoginProvider,
            AbpEmailTwoFactorTokenProvider.ProviderName + ":" + TwoFactorPurpose);
        stored.ShouldNotBeNull();

        var separator = stored.LastIndexOf('|');
        separator.ShouldBeGreaterThan(0);
        var secondsPart = stored.Substring(separator + 1);
        long.TryParse(secondsPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var storedExpiresUnix)
            .ShouldBeTrue();

        storedExpiresUnix.ShouldBeGreaterThanOrEqualTo(beforeGenerate + (long)customLifespan.TotalSeconds);
        storedExpiresUnix.ShouldBeLessThanOrEqualTo(afterGenerate + (long)customLifespan.TotalSeconds);

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Direct_Provider_Generate_And_Validate_Should_Round_Trip()
    {
        using var uow = UnitOfWorkManager.Begin();

        var provider = GetRequiredService<AbpEmailTwoFactorTokenProvider>();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await provider.GenerateAsync(TwoFactorPurpose, UserManager, user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await provider.ValidateAsync(TwoFactorPurpose, code, UserManager, user)).ShouldBeTrue();

        // Consumed by the previous successful validation.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await provider.ValidateAsync(TwoFactorPurpose, code, UserManager, user)).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public void Provider_Name_Should_Be_The_Published_Constant()
    {
        var provider = GetRequiredService<AbpEmailTwoFactorTokenProvider>();

        provider.Name.ShouldBe(AbpEmailTwoFactorTokenProvider.ProviderName);
        AbpEmailTwoFactorTokenProvider.ProviderName.ShouldBe("AbpEmailTwoFactor");
    }

    [Fact]
    public async Task Unprotectable_Stored_Payload_From_Wrong_Protector_Should_Fail_And_Cleanup()
    {
        // Protect a code under a DIFFERENT DataProtection purpose chain to simulate data
        // carried over from another module/provider. ValidateAsync must refuse it (Unprotect
        // throws) and clean up the stored entry.
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);
        var tokenName = GetTokenName(TwoFactorPurpose);

        var dpp = GetRequiredService<IDataProtectionProvider>();
        var wrongProtector = dpp.CreateProtector("some-unrelated-purpose");
        var wrongPayload = wrongProtector.Protect("123456");

        var futureSeconds = new DateTimeOffset(Clock.Now.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(Clock.Now.AddMinutes(1), DateTimeKind.Local)
                : Clock.Now.AddMinutes(1))
            .ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, wrongPayload + "|" + futureSeconds))
            .Succeeded.ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName))
            .ShouldBeNull();

        await uow.CompleteAsync();
    }
}
