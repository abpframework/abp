using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Abstract base class that exercises the common behaviour of every
/// <see cref="AbpTwoFactorTokenProvider"/> subclass. Concrete subclasses wire up
/// the Identity API calls that route to the provider under test.
/// </summary>
public abstract class AbpTwoFactorTokenProviderTestBase : AbpIdentityAspNetCoreTestBase
{
    protected const string TwoFactorPurpose = "TwoFactor";
    protected const string OtherPurpose = "SomeOtherPurpose";

    protected IIdentityUserRepository UserRepository { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentityTestData TestData { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IClock Clock { get; }

    protected AbpTwoFactorTokenProviderTestBase()
    {
        UserRepository = GetRequiredService<IIdentityUserRepository>();
        UserManager = GetRequiredService<IdentityUserManager>();
        TestData = GetRequiredService<IdentityTestData>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        Clock = GetRequiredService<IClock>();
    }

    /// <summary>Identity provider name (e.g. "Email", "Phone") routed to the provider under test.</summary>
    protected abstract string GetTokenProviderName();

    /// <summary>Internal <see cref="AbpTwoFactorTokenProvider.Name"/> used as the stored-token key prefix.</summary>
    protected abstract string GetInternalProviderName();

    protected virtual Task<string> GenerateTokenAsync(IdentityUser user, string purpose = TwoFactorPurpose)
        => UserManager.GenerateUserTokenAsync(user, GetTokenProviderName(), purpose);

    protected virtual Task<bool> VerifyTokenAsync(IdentityUser user, string token, string purpose = TwoFactorPurpose)
        => UserManager.VerifyUserTokenAsync(user, GetTokenProviderName(), purpose, token);

    protected string GetTokenName(string purpose) => GetInternalProviderName() + ":" + purpose;

    /// <summary>Deterministically produce a code that is guaranteed not to equal <paramref name="code"/>.</summary>
    private static string DifferentCode(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return "000000";
        }

        var firstChar = code[0];
        var replacement = firstChar == '9' ? '0' : (char)(firstChar + 1);
        return replacement + code.Substring(1);
    }

    private long ToUnixSeconds(DateTime moment)
    {
        if (moment.Kind == DateTimeKind.Unspecified)
        {
            moment = DateTime.SpecifyKind(moment, DateTimeKind.Local);
        }

        return new DateTimeOffset(moment).ToUnixTimeSeconds();
    }

    [Fact]
    public async Task Generate_Should_Produce_Six_Digit_Numeric_Code()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        code.ShouldNotBeNullOrEmpty();
        code.Length.ShouldBe(6);
        code.ShouldMatch(@"^\d{6}$");

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Generate_And_Verify_Token_Should_Succeed()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Invalid_Token_Should_Fail_Verification()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, DifferentCode(code))).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Wrong_Code_Should_Not_Consume_Stored_Entry()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        // A wrong attempt must leave the stored entry intact so the user can retry.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, DifferentCode(code))).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Successful_Verification_Should_Consume_The_Code()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeTrue();

        // Single-use: the same code must not validate twice.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Second_Token_Generation_Should_Invalidate_First_Token()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var firstCode = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var secondCode = await GenerateTokenAsync(user);

        // Even if first/second codes happen to collide numerically, the first was
        // generated against a now-overwritten stored entry, so verifying the FIRST
        // code against the NEW stored entry is what counts. We only assert the
        // old token no longer validates and the new one does.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        if (firstCode != secondCode)
        {
            (await VerifyTokenAsync(user, firstCode)).ShouldBeFalse();
            user = await UserRepository.GetAsync(TestData.UserJohnId);
        }

        (await VerifyTokenAsync(user, secondCode)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Expired_Token_Should_Fail_And_Be_Cleaned_Up()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        // Keep the protected payload but rewrite the expiration into the past.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var tokenName = GetTokenName(TwoFactorPurpose);
        var stored = await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName);
        stored.ShouldNotBeNull();

        var separator = stored.LastIndexOf('|');
        separator.ShouldBeGreaterThan(0);
        var protectedPart = stored.Substring(0, separator);
        var expiredValue = protectedPart + "|" +
            ToUnixSeconds(Clock.Now.AddMinutes(-1)).ToString(CultureInfo.InvariantCulture);

        (await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, expiredValue))
            .Succeeded.ShouldBeTrue();

        // Verification must fail and also wipe the expired entry.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var remaining = await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName);
        remaining.ShouldBeNull();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Corrupted_Stored_Value_Should_Return_False_Instead_Of_Throwing()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        var tokenName = GetTokenName(TwoFactorPurpose);

        // Overwrite with a malformed value (no separator, garbage chars).
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, "not-a-valid-stored-value"))
            .Succeeded.ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        // Corrupt entry is cleaned up.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var remaining = await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName);
        remaining.ShouldBeNull();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Different_Purposes_Should_Be_Isolated()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var twoFactorCode = await GenerateTokenAsync(user, TwoFactorPurpose);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var otherCode = await GenerateTokenAsync(user, OtherPurpose);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        // Wrong-purpose validation must fail.
        (await VerifyTokenAsync(user, otherCode, TwoFactorPurpose)).ShouldBeFalse();
        (await VerifyTokenAsync(user, twoFactorCode, OtherPurpose)).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        // Correct purposes still work independently.
        (await VerifyTokenAsync(user, twoFactorCode, TwoFactorPurpose)).ShouldBeTrue();
        (await VerifyTokenAsync(user, otherCode, OtherPurpose)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Stored_Token_Data_Should_Persist_Across_UnitOfWork_Boundaries()
    {
        string code;

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            code = await GenerateTokenAsync(user);
            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(user, code)).ShouldBeTrue();
            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Verify_Without_Generate_Should_Fail()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, "123456")).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Empty_Token_Should_Fail_Verification()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, string.Empty)).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Different_Users_Should_Be_Isolated()
    {
        using var uow = UnitOfWorkManager.Begin();

        var john = await UserRepository.GetAsync(TestData.UserJohnId);
        var johnCode = await GenerateTokenAsync(john);

        var david = await UserRepository.GetAsync(TestData.UserDavidId);
        var davidCode = await GenerateTokenAsync(david);

        // Neither user's code should validate for the other (assuming distinct codes;
        // even if they collide by chance the FixedTimeEquals path still runs, but the
        // DataProtection payload is bound to a different user-scoped record).
        if (johnCode != davidCode)
        {
            john = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(john, davidCode)).ShouldBeFalse();

            david = await UserRepository.GetAsync(TestData.UserDavidId);
            (await VerifyTokenAsync(david, johnCode)).ShouldBeFalse();
        }

        // Both still validate against their own user, proving that generating David's
        // did not consume John's stored entry (or vice versa).
        john = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(john, johnCode)).ShouldBeTrue();

        david = await UserRepository.GetAsync(TestData.UserDavidId);
        (await VerifyTokenAsync(david, davidCode)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Multiple_Purposes_Can_Coexist_And_Consume_Independently()
    {
        using var uow = UnitOfWorkManager.Begin();

        const string purposeA = "PurposeA";
        const string purposeB = "PurposeB";
        const string purposeC = "PurposeC";

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var codeA = await GenerateTokenAsync(user, purposeA);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var codeB = await GenerateTokenAsync(user, purposeB);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var codeC = await GenerateTokenAsync(user, purposeC);

        // Consume B; A and C must still be valid.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, codeB, purposeB)).ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, codeA, purposeA)).ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, codeC, purposeC)).ShouldBeTrue();

        // Each is single-use and now consumed.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, codeA, purposeA)).ShouldBeFalse();
        (await VerifyTokenAsync(user, codeB, purposeB)).ShouldBeFalse();
        (await VerifyTokenAsync(user, codeC, purposeC)).ShouldBeFalse();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Stored_Value_Missing_Separator_Should_Return_False_And_Cleanup()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);
        var tokenName = GetTokenName(TwoFactorPurpose);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, "no-separator-here"))
            .Succeeded.ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName))
            .ShouldBeNull();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Stored_Value_With_Non_Numeric_Expiration_Should_Return_False_And_Cleanup()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);
        var tokenName = GetTokenName(TwoFactorPurpose);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, "validlookingpayload|not-a-number"))
            .Succeeded.ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName))
            .ShouldBeNull();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Stored_Value_With_Unprotectable_Payload_Should_Return_False_And_Cleanup()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);
        var tokenName = GetTokenName(TwoFactorPurpose);

        var futureSeconds = ToUnixSeconds(Clock.Now.AddMinutes(1)).ToString(CultureInfo.InvariantCulture);

        // Valid expiration, but the protected payload is not a real DataProtection output.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, $"not-a-real-protected-payload|{futureSeconds}"))
            .Succeeded.ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeFalse();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName))
            .ShouldBeNull();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Generation_Should_Not_Touch_Pre_Existing_Unrelated_Tokens()
    {
        // John already has a pre-seeded token ("test-provider"/"test-name" = "test-value").
        // Generating a 2FA code must not interfere with it.
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var preExisting = await UserManager.GetAuthenticationTokenAsync(user, "test-provider", "test-name");
        preExisting.ShouldBe("test-value");

        await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(user, "test-provider", "test-name"))
            .ShouldBe("test-value");

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Successful_Verification_Should_Not_Touch_Pre_Existing_Unrelated_Tokens()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, code)).ShouldBeTrue();

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(user, "test-provider", "test-name"))
            .ShouldBe("test-value");

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Token_Should_Be_Stored_Under_Internal_Login_Provider_Name()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var stored = await UserManager.GetAuthenticationTokenAsync(
            user,
            AbpTwoFactorTokenProvider.InternalLoginProvider,
            GetTokenName(TwoFactorPurpose));

        stored.ShouldNotBeNullOrEmpty();
        stored.ShouldContain("|");

        // Must not leak under the Identity provider name itself.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await UserManager.GetAuthenticationTokenAsync(user, GetTokenProviderName(), TwoFactorPurpose))
            .ShouldBeNull();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Stored_Value_Should_Be_Encrypted_Not_The_Raw_Code()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var code = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var stored = await UserManager.GetAuthenticationTokenAsync(
            user,
            AbpTwoFactorTokenProvider.InternalLoginProvider,
            GetTokenName(TwoFactorPurpose));

        stored.ShouldNotBeNull();
        stored.ShouldNotContain(code);

        // DataProtection output is substantially longer than a 6-digit code, which is a
        // cheap sanity check that we're not accidentally storing the plaintext.
        var protectedPart = stored.Substring(0, stored.LastIndexOf('|'));
        protectedPart.Length.ShouldBeGreaterThan(code.Length * 4);

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Consecutive_Generations_Should_Produce_Different_Protected_Payloads()
    {
        // DataProtection embeds per-call randomness, so even if the two underlying codes
        // collide, their stored payloads will differ. This replaces the old
        // "two generations produce different codes" assertion, which had a ~1e-6 flake rate.
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var firstStored = await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, GetTokenName(TwoFactorPurpose));

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var secondStored = await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, GetTokenName(TwoFactorPurpose));

        firstStored.ShouldNotBeNull();
        secondStored.ShouldNotBeNull();
        secondStored.ShouldNotBe(firstStored);

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Expired_Entry_Should_Not_Block_Future_Generation()
    {
        using var uow = UnitOfWorkManager.Begin();

        var user = await UserRepository.GetAsync(TestData.UserJohnId);
        var firstCode = await GenerateTokenAsync(user);

        // Force first entry into the past.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var tokenName = GetTokenName(TwoFactorPurpose);
        var stored = await UserManager.GetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName);
        var protectedPart = stored!.Substring(0, stored.LastIndexOf('|'));
        var expiredValue = protectedPart + "|" +
            ToUnixSeconds(Clock.Now.AddMinutes(-1)).ToString(CultureInfo.InvariantCulture);
        await UserManager.SetAuthenticationTokenAsync(
            user, AbpTwoFactorTokenProvider.InternalLoginProvider, tokenName, expiredValue);

        // Verifying the expired code fails and cleans up.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, firstCode)).ShouldBeFalse();

        // New generation must succeed and verify.
        user = await UserRepository.GetAsync(TestData.UserJohnId);
        var secondCode = await GenerateTokenAsync(user);

        user = await UserRepository.GetAsync(TestData.UserJohnId);
        (await VerifyTokenAsync(user, secondCode)).ShouldBeTrue();

        await uow.CompleteAsync();
    }
}
