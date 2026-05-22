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

public class AbpPhoneNumberTwoFactorTokenProvider_Tests : AbpTwoFactorTokenProviderTestBase
{
    protected override string GetTokenProviderName() => TokenOptions.DefaultPhoneProvider;

    protected override string GetInternalProviderName() => AbpPhoneNumberTwoFactorTokenProvider.ProviderName;

    [Fact]
    public void AbpPhoneNumberTwoFactorTokenProvider_Should_Replace_Default_Phone_Provider()
    {
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>().Value;

        identityOptions.Tokens.ProviderMap.ShouldContainKey(TokenOptions.DefaultPhoneProvider);
        identityOptions.Tokens.ProviderMap[TokenOptions.DefaultPhoneProvider].ProviderType
            .ShouldBe(typeof(AbpPhoneNumberTwoFactorTokenProvider));
    }

    [Fact]
    public void Default_Options_Should_Match_Documented_Defaults()
    {
        var options = GetRequiredService<IOptions<AbpPhoneNumberTwoFactorTokenProviderOptions>>().Value;

        options.CodeLength.ShouldBe(6);
        options.TokenLifespan.ShouldBe(TimeSpan.FromMinutes(3));
    }

    [Fact]
    public async Task CanGenerateTwoFactorTokenAsync_Should_Require_Confirmed_Phone_Number()
    {
        using var uow = UnitOfWorkManager.Begin();

        var provider = GetRequiredService<AbpPhoneNumberTwoFactorTokenProvider>();
        var john = await UserRepository.GetAsync(TestData.UserJohnId);

        // No phone number set by the seed.
        (await provider.CanGenerateTwoFactorTokenAsync(UserManager, john)).ShouldBeFalse();

        (await UserManager.SetPhoneNumberAsync(john, "+1-555-0100")).Succeeded.ShouldBeTrue();

        john = await UserRepository.GetAsync(TestData.UserJohnId);
        // Phone present but not confirmed yet.
        (await provider.CanGenerateTwoFactorTokenAsync(UserManager, john)).ShouldBeFalse();

        john.SetPhoneNumberConfirmed(true);
        (await UserManager.UpdateAsync(john)).Succeeded.ShouldBeTrue();

        john = await UserRepository.GetAsync(TestData.UserJohnId);
        (await provider.CanGenerateTwoFactorTokenAsync(UserManager, john)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task ChangePhoneNumber_Token_Should_Be_Single_Use()
    {
        // IdentityOptions.Tokens.ChangePhoneNumberTokenProvider defaults to the same
        // "Phone" provider name, so GenerateChangePhoneNumberTokenAsync now routes
        // through AbpPhoneNumberTwoFactorTokenProvider and inherits single-use semantics.
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        const string newPhone = "+1-555-0199";
        var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, newPhone);

        token.ShouldNotBeNullOrEmpty();
        token.Length.ShouldBe(6);
        token.ShouldMatch(@"^\d{6}$");

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.VerifyChangePhoneNumberTokenAsync(user, token, newPhone)).ShouldBeTrue();

        // Single-use: second verification must fail.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.VerifyChangePhoneNumberTokenAsync(user, token, newPhone)).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task ChangePhoneNumber_Token_Should_Be_Bound_To_The_Target_Phone_Number()
    {
        // Purpose is "ChangePhoneNumber:{phoneNumber}" so a token issued for one
        // target phone number must not validate against a different one.
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, "+1-555-0111");

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.VerifyChangePhoneNumberTokenAsync(user, token, "+1-555-0222")).ShouldBeFalse();

        // Original target still works.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.VerifyChangePhoneNumberTokenAsync(user, token, "+1-555-0111")).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Phone_Code_Should_Survive_Security_Stamp_Change()
    {
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

        var customProvider = new AbpPhoneNumberTwoFactorTokenProvider(
            Microsoft.Extensions.Options.Options.Create(new AbpPhoneNumberTwoFactorTokenProviderOptions { CodeLength = 4 }),
            UserRepository,
            GetRequiredService<ICancellationTokenProvider>(),
            Clock,
            GetRequiredService<IDataProtectionProvider>());

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await customProvider.GenerateAsync(TwoFactorPurpose, UserManager, user);

        code.Length.ShouldBe(4);
        code.ShouldMatch(@"^\d{4}$");

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await customProvider.ValidateAsync(TwoFactorPurpose, code, UserManager, user)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Custom_TokenLifespan_Should_Be_Honored()
    {
        using var uow = UnitOfWorkManager.Begin();

        var customLifespan = TimeSpan.FromMinutes(15);

        var customProvider = new AbpPhoneNumberTwoFactorTokenProvider(
            Microsoft.Extensions.Options.Options.Create(new AbpPhoneNumberTwoFactorTokenProviderOptions { TokenLifespan = customLifespan }),
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
            AbpPhoneNumberTwoFactorTokenProvider.ProviderName + ":" + TwoFactorPurpose);
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

        var provider = GetRequiredService<AbpPhoneNumberTwoFactorTokenProvider>();

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
        var provider = GetRequiredService<AbpPhoneNumberTwoFactorTokenProvider>();

        provider.Name.ShouldBe(AbpPhoneNumberTwoFactorTokenProvider.ProviderName);
        AbpPhoneNumberTwoFactorTokenProvider.ProviderName.ShouldBe("AbpPhoneNumberTwoFactor");
    }
}
